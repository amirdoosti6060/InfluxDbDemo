using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;

namespace InfluxDbDemo
{
    public class DbClient: IDisposable
    {
        private readonly string _url;
        private readonly string _token;
        private readonly string _bucket;
        private readonly string _organization;

        private readonly InfluxDBClient _client;
        private readonly Random _random;

        public DbClient(string url, string token, string bucket, string organization) 
        {
            _url = url;
            _token = token;
            _bucket = bucket;
            _organization = organization;

            _random = new Random(DateTime.Now.Millisecond);

            _client = new InfluxDBClient(_url, _token); 
        }

        public void InsertByPointData()
        {
            var temperature = (double)_random.Next(20, 40) + _random.NextDouble();

            var point = PointData
                            .Measurement("worldtemperature")
                            .Tag("city", "Tehran")
                            .Field("temperature", temperature);

            using (var writeApi = _client.GetWriteApi())
            {
                writeApi.WritePoint(point: point, bucket: _bucket, org: _organization);
            }

            Console.WriteLine($"city: Tehran, Temperature:{temperature}");
        }

        public void InsertByLineProtocol()
        {
            var temperature = (double)_random.Next(20, 40) + _random.NextDouble();
            string line = $"worldtemperature,city=London temperature={temperature}";

            using (var writeApi = _client.GetWriteApi())
            {
                writeApi.WriteRecord(record: line, bucket: _bucket, org: _organization);
            }

            Console.WriteLine($"city: London, Temperature:{temperature}");
        }

        public async Task QueryByFlux()
        {
            var query = "from(bucket: \"DefaultBucket\") |> range(start: -10d) |> filter(fn: (r) => r._measurement == \"worldtemperature\") ";
            var tables = await _client.GetQueryApi().QueryAsync(query, _organization);

            Console.WriteLine("All world temperature data from 10 days ago until now:\n");
            Console.WriteLine("Timestamp\t\tcity\ttemperature");
            foreach (var record in tables.SelectMany(table => table.Records))
            {
                Console.WriteLine($"{record.GetTimeInDateTime()}\t{record.GetValueByKey("city")}\t{record.GetValueByKey("_value")}");
            }
        }

        public void Dispose() 
        { 
            _client.Dispose();
        }
    }
}
