using CMSProj.DataLayer.UrlServices;
using CMSProj.DataLayer.UrlServices.Factories;
using CMSProj.SubSystems.BackGroundServices.UrlUpdate;

using Microsoft.Extensions.Logging;

using NuGet.Protocol;

using System.Collections;

namespace PathTests
{
    public class PathUpdateTest : IClassFixture<RouteTestFixture>
    {
        readonly RouteTestFixture _fixture;
        readonly IActiveRouteManager _sut;
        public PathUpdateTest(RouteTestFixture fixture)
        {
            _fixture = fixture;
            _sut = fixture.Sut;
        }

        public IActiveRouteManager Sut { get; private set; }

        [Theory]
        [ClassData(typeof(RoutesTestData))]
        public void TestUpdateRoutes(string route, Guid guid)
        {
            //arrange
            var expected = guid;
            _sut.Initialize();
            _fixture.UpdatePage(route, guid);

            //act
            _sut.GetAvailableRoutes();
            var actual = _sut.GetPageGuid(route);

            //assert
            Assert.Equal(expected, actual);

        }
        [Theory]
        [ClassData(typeof(RoutesTestData))]
        public void TestAddRoutesData(string route, Guid id)
        {
            //arrange
            var expected = id;
            _sut.Initialize();
            _fixture.AddNewPage(route, id);

            //act
            _sut.GetAvailableRoutes();
            var actual = _sut.GetPageGuid(route);

            Assert.Equal(expected, actual);
        }
        [Theory]
        [ClassData(typeof(RemoveRoutesTestData))]
        public void TestRemoveRoutesData(string route, Guid id)
        {
            //arrange
            _sut.Initialize();
            _fixture.RemovePage(route);

            //act
            _sut.GetAvailableRoutes();
            var actual = _sut.GetPageGuid(route);

            Assert.Null(actual);
        }
    }
    public class InitializeRoutes : IEnumerable<object[]>
    {
        private List<object[]> _initData { get; } =
            [
            ["Home", new Guid("519a5167-8b48-4c0e-aba2-782267044f34")],
            ["Other", new Guid("d118dba6-959d-49a2-8563-b6fa33260c67")]
        ];
        public IEnumerator<object[]> GetEnumerator()
        {
            return _initData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class RemoveRoutesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return new InitializeRoutes().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class AddRoutesTestData : IEnumerable<object[]>
    {
        private List<object[]> _testCases { get; } =
            [
            ["Sdfkjwo", Guid.NewGuid()],
            ["f02lksdv", Guid.NewGuid()],
            ["vmlas293ur", Guid.NewGuid()],
            ["092vkf123fa", Guid.NewGuid()]
            ];
        public IEnumerator<object[]> GetEnumerator()
        {
            return _testCases.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class RoutesTestData : IEnumerable<object[]>
    {
        private List<object[]> _testCases { get; } = [
            ["Home", Guid.NewGuid()],
            ["Hello", Guid.NewGuid()],
            ["Other", Guid.NewGuid()],
            ["SomeString", Guid.NewGuid()]
        ];
        public IEnumerator<object[]> GetEnumerator()
        {
            return _testCases.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class RouteTestFixture 
    {
        public IActiveRouteManager Sut { get; private set; }
        private IUrlRepository _repoMock;

        public RouteTestFixture()
        {
            _repoMock = new RepoMock();
            Sut = new ActiveRoutesManager(
                new RouteMatcherFactory(),
                _repoMock,
                new RouteManagerWorkerState(
                    new WorkerResultFactory<int>(),
                    new LogMessageFactory(),
                    new WorkerLoggerMock()), 
                new ActiveRoutesLoggerMock());

        }
        public void AddNewPage(string page, Guid id)
        {
            ((RepoMock)_repoMock).AddUrl(page, id);
        }
        public void RemovePage(string page)
        {
            ((RepoMock)_repoMock).RemoveUrl(page);
        }
        public void UpdatePage(string page, Guid id)
        {
            ((RepoMock)_repoMock).RemoveUrl(page);
            ((RepoMock)_repoMock).AddUrl(page, id);
        }
    }

    internal class RepoMock : IUrlRepository
    {
        private Dictionary<string, Guid> dbMock { get; } = new();

        public void AddUrl(string page, Guid id)
        {
            if(!dbMock.TryGetValue(page, out var res))
                dbMock.Add(page, id);
            dbMock[page] = id;
        }
        public void RemoveUrl(string page)
        {
            if(dbMock.TryGetValue(page, out var res))
                dbMock.Remove(page);
        }
        private ICollection<UrlGuidAdapter> Create()
        {
            if (dbMock.Count < 1) return Init();
            return Read();
        }
        private ICollection<UrlGuidAdapter> Read()
        {
            var list = new List<UrlGuidAdapter>();
            foreach(var i in dbMock)
            {
                list.Add(new UrlGuidAdapter { Guid = i.Value, PageUrl = i.Key });
            }
            return list;
        }
        private ICollection<UrlGuidAdapter> Init()
        {
            var list = new List<UrlGuidAdapter>();
            foreach(var i in new InitializeRoutes())
            {
                list.Add(new UrlGuidAdapter() { Guid = (Guid)i[1], PageUrl = (string)i[0] });
                dbMock.Add((string)i[0], (Guid)i[1]);
                Console.WriteLine((string)i[0], (Guid)i[1]);
            }
            return list;
        }

        public ICollection<UrlGuidAdapter> GetUrls()
        {
            return Create();
        }

        public async Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token)
        {
            await Task.Delay(new Random().Next(200, 2000));
            return Create();
        }
    }
    internal class disposable : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("dispsed");
        }
    }
    internal class WorkerLoggerMock : ILogger<IWorkResultOrchestrator<WorkerResult<int>>>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return new disposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Console.WriteLine("logmess");
        }
    }
    internal class ActiveRoutesLoggerMock : ILogger<ActiveRoutesManager>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return new disposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Console.WriteLine("logged");
        }
    }
}
