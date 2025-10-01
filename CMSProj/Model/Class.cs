namespace CMSProj.Model
{
    using ContentDatabase;
    using ContentDatabase.Model;

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;

    public class ContactFormVm
    {
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(14)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Message { get; set; }
    }
    public interface IContactRepo
    {
        Task<int> CountAsync(CancellationToken ct = default);
        Task AddAsync(ContatUser entity, CancellationToken ct = default);
        Task<List<ContatUser>> ListAsync(CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }

    public sealed class ContactRepo : IContactRepo
    {
        private readonly ContentContext _db;
        public ContactRepo(ContentContext db) { _db = db; }

        public Task<int> CountAsync(CancellationToken ct = default)
            => _db.ContacUsers.CountAsync(ct);

        public async Task AddAsync(ContatUser entity, CancellationToken ct = default)
        {
            await _db.ContacUsers.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);
        }

        public Task<List<ContatUser>> ListAsync(CancellationToken ct = default)
            => _db.ContacUsers.OrderByDescending(x => x.CreatedUtc).ToListAsync(ct);

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var stub = new ContatUser { Id = id };
            _db.Attach(stub);
            _db.Remove(stub);
            await _db.SaveChangesAsync(ct);
        }
    }

    public interface IContactManager
    {
        Task<ContactResultVm> CreateAsync(ContactFormVm vm, CancellationToken ct = default);
        Task<List<ContatUser>> GetAllAsync(CancellationToken ct = default);
        Task RemoveAsync(Guid id, CancellationToken ct = default);
    }

    public sealed class ContactManager : IContactManager
    {
        private readonly IContactRepo _repo;
        public ContactManager(IContactRepo repo) { _repo = repo; }

        public async Task<ContactResultVm> CreateAsync(ContactFormVm vm, CancellationToken ct = default)
        {
            var id = Guid.NewGuid();
            var entity = new ContatUser
            {
                Id = id,
                Email = vm.Email.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(vm.PhoneNumber) ? null : vm.PhoneNumber.Trim(),
                Name = vm.Name.Trim(),
                Message = vm.Message.Trim(),
                CreatedUtc = DateTime.UtcNow
            };
            var prior = await _repo.CountAsync(ct);
            await _repo.AddAsync(entity, ct);
            return new ContactResultVm
            {
                Email = entity.Email,
                Name = entity.Name,
                Position = prior + 1,
                Id = id
            };
        }

        public Task<List<ContatUser>> GetAllAsync(CancellationToken ct = default) => _repo.ListAsync(ct);
        public Task RemoveAsync(Guid id, CancellationToken ct = default) => _repo.DeleteAsync(id, ct);
    }
    public static class ContactFeature
    {
        public static IServiceCollection AddContactFeature(this IServiceCollection services)
        {
            services.AddScoped<IContactRepo, ContactRepo>();
            services.AddScoped<IContactManager, ContactManager>();
            return services;
        }
    }
}
