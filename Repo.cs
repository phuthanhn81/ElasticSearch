using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elasticsearch
{
    public class Repo
    {
        public ElasticClient EsClient()
        {
            string HostElasticSearch = System.Configuration.ConfigurationManager.AppSettings["HostElasticSearch"];
        
            var nodes = new Uri[]
            {
                new Uri(HostElasticSearch),
            };

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            var elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }

        public bool IsUnicode(string str)
        {
            return (Encoding.UTF8.GetByteCount(str) != str.Length);
        }

        public List<ArticleSearchElastic> Query(string keyword, int currentPage, int pageSize)
        {
            ISearchResponse<ArticleSearchElastic> list;
            if (IsUnicode(keyword))
            {
                // Analyzer ở đây chỉ lấy những thằng nào giống với yêu cầu chứ ko phải search all (nó là 1 cái yêu cầu chứ ko phải ascii field đó)
                // set lại size để lấy hơn 10000 rows nhưng do quá lớn nên sẽ sập server
                list = EsClient().Search<ArticleSearchElastic>(s => s
                                    .Index("article").From((currentPage - 1) * pageSize)
                                    .Type("_doc")
                                    .Query(n => n
                                        .Bool(x => x
                                            .Must(a => a
                                                .Match(b => b
                                                    .Field(c => c.statusid)
                                                        .Query("-1")), a => a
                                                .Bool(b => b
                                                   .Should(c => c
                                                        .MatchPhrase(d => d
                                                       .Field(e => e.Title)
                                                       .Query(keyword)
                                                   ), d => d
                                                    .MatchPhrase(e => e
                                                        .Field(f => f.Head)
                                                        .Query(keyword))
                                                        )), a => a
                                                .Bool(b => b
                                                    .Must(c => c
                                                        .DateRange(d => d
                                                        .Field(k => k.PublishTime).Format("yyyy-MM-dd HH:mm:ss")
                                                        .LessThanOrEquals(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))))))))
                                    .Sort(a => a.Descending(m => m.PublishTime)));
            }
            else
            {
                list = EsClient().Search<ArticleSearchElastic>(s => s
                                    .Index("article").From((currentPage - 1) * pageSize)
                                    .Type("_doc")
                                    .Query(n => n
                                        .Bool(x => x
                                            .Must(a => a
                                                .Match(b => b
                                                    .Field(c => c.statusid)
                                                        .Query("-1")), a => a
                                                .Bool(b => b
                                                   .Should(c => c
                                                        .MatchPhrase(d => d
                                                       .Field(e => e.ascii_title)
                                                       .Query(keyword)
                                                   ), d => d
                                                    .MatchPhrase(e => e
                                                        .Field(f => f.ascii_head)
                                                        .Query(keyword))
                                                        )), a => a
                                                .Bool(b => b
                                                    .Must(c => c
                                                        .DateRange(d => d
                                                        .Field(k => k.PublishTime).Format("yyyy-MM-dd HH:mm:ss")
                                                        .LessThanOrEquals(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))))))))
                                    .Sort(a => a.Descending(m => m.PublishTime)));
            }

           

            var datasend = (from hits in list.Hits
                                      select hits.Source).ToList();

            
            return datasend;

        }

        public long total(string keyword)
        {
            long total = 0;
            if (IsUnicode(keyword))
            {
                var list = EsClient().Count<ArticleSearchElastic>(s => s
                                .Index("article")
                                .Type("_doc")
                                .Query(n => n
                                    .Bool(x => x
                                        .Must(a => a
                                            .Match(b => b
                                                .Field(c => c.statusid)
                                                    .Query("-1")), a => a
                                            .Bool(b => b
                                               .Should(c => c
                                                    .MatchPhrase(d => d
                                                   .Field(e => e.Title)
                                                   .Query(keyword)
                                               ), d => d
                                                .MatchPhrase(e => e
                                                    .Field(f => f.Head)
                                                    .Query(keyword))
                                                    )), a => a
                                            .DateRange(b => b.
                                                Field(c => c.PublishTime).Format("yyyy-MM-dd HH:mm:ss")
                                                .LessThanOrEquals(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))))));

                total = list.Count;
            }
            else
            {
                var list = EsClient().Count<ArticleSearchElastic>(s => s
                                .Index("article")
                                .Type("_doc")
                                .Query(n => n
                                    .Bool(x => x
                                        .Must(a => a
                                            .Match(b => b
                                                .Field(c => c.statusid)
                                                    .Query("-1")), a => a
                                            .Bool(b => b
                                               .Should(c => c
                                                    .MatchPhrase(d => d
                                                   .Field(e => e.ascii_title)
                                                   .Query(keyword)
                                               ), d => d
                                                .MatchPhrase(e => e
                                                    .Field(f => f.ascii_head)
                                                    .Query(keyword))
                                                    )), a => a
                                            .DateRange(b => b.
                                                Field(c => c.PublishTime).Format("yyyy-MM-dd HH:mm:ss")
                                                .LessThanOrEquals(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))))));
                total = list.Count;
            }
            return total;
        }
    }
}


