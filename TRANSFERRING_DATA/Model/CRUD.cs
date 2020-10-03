using Elasticsearch.Net;
using Nest;
using System;

namespace Model
{
    public class CRUD
    {
        public static ElasticClient EsClient()
        {
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
            };

            var ConnectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(ConnectionPool);
            var elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }

        public void _CRUD(int option)
        {
            if (option == 1)
            {
                Content content = new Content();
                content.ID = 3001;
                content.Birth = DateTime.UtcNow;
                content.Name = "3001";

                var response = EsClient().Index(content, i => i
                              .Index("content")
                              .Type("doc")
                              .Refresh(Refresh.True));
            }
            else if (option == 2)
            {
                Content content = new Content();
                content.ID = 08011996;
                content.Birth = DateTime.UtcNow;
                content.Name = "08011996";

                var response = EsClient().Index(content, i => i
                              .Index("content")
                              .Type("doc")
                              .Id(10)
                              .Refresh(Refresh.True));
            }
            else
            {
                var response = EsClient().Delete<Content>(4, d => d
                              .Index("content")
                              .Type("doc"));
            }
        }
    }
}
