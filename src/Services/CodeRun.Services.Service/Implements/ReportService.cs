using CodeRun.Services.Domain.IRepository.Web;
using CodeRun.Services.IService.Dtos;
using CodeRun.Services.IService.Interfaces;

namespace CodeRun.Services.Service.Implements
{
    public class ReportService : ServiceBase, IReportService
    {
        private readonly IDapperRepository _dapperRepository;

        public ReportService(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        /// <summary>
        /// 获取数据概括
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsDataDto>> GetAllDataAsync()
        {
            DateTime now = DateTime.Today;
            DateTime last = now.AddDays(-1);

            const string sql = @"
                                -- App下载
                                SELECT COUNT(*) FROM AppDevices; 
                                SELECT COUNT(*) FROM AppDevices WHERE CreatedTime >= @start AND CreatedTime <= @end;
        
                                -- 注册用户
                                SELECT COUNT(*) FROM AppUserInfos; 
                                SELECT COUNT(*) FROM AppUserInfos WHERE JoinTime >= @start AND JoinTime <= @end;
        
                                -- 八股文
                                SELECT COUNT(*) FROM QuestionInfos; 
                                SELECT COUNT(*) FROM QuestionInfos WHERE CreatedTime >= @start AND CreatedTime <= @end;
        
                                -- 考题
                                SELECT COUNT(*) FROM ExamQuestions; 
                                SELECT COUNT(*) FROM ExamQuestions WHERE CreatedTime >= @start AND CreatedTime <= @end;
        
                                -- 分享
                                SELECT COUNT(*) FROM ShareInfos; 
                                SELECT COUNT(*) FROM ShareInfos WHERE CreatedTime >= @start AND CreatedTime <= @end;
        
                                -- 反馈
                                SELECT COUNT(*) FROM AppFeedbacks; 
                                SELECT COUNT(*) FROM AppFeedbacks WHERE CreatedTime >= @start AND CreatedTime <= @end;";

            using var multi = await _dapperRepository.QueryMultipleAsync(sql, new { start = last, end = now });

            var titles = new[] { "App下载", "注册用户", "八股文", "考题", "分享", "反馈" };
            var results = new List<StatisticsDataDto>();

            for (int i = 0; i < titles.Length; i++)
            {
                results.Add(new StatisticsDataDto
                {
                    StatisticsName = titles[i],
                    Count = await multi.ReadFirstOrDefaultAsync<int>(),
                    PerCount = await multi.ReadFirstOrDefaultAsync<int>()
                });
            }

            return results;
        }

        /// <summary>
        /// 获取app注册统计数据(近一周的数据)
        /// </summary>
        /// <returns></returns>
        public async Task<StatisticsDataWeekDto> GetAppWeekDataAsync()
        {
            var weeks = GetWeeks();

            StatisticsDataWeekDto statistics = new StatisticsDataWeekDto
            {
                DataList = weeks
            };

            StatisticsDataDto download = new StatisticsDataDto
            {
                StatisticsName = "App下载"
            };
            StatisticsDataDto register = new StatisticsDataDto
            {
                StatisticsName = "App注册"
            };

            foreach (var item in weeks)
            {
                DateTime start = DateTime.Parse(item);
                DateTime end = start.AddDays(1);

                string sql = $"SELECT COUNT(*) FROM AppDevices WHERE CreatedTime >= @start AND CreatedTime <= @end;" +
                    $"SELECT COUNT(*) FROM AppUserInfos WHERE JoinTime >= @start AND JoinTime <= @end;";

                using var multi = await _dapperRepository.QueryMultipleAsync(sql, new { start = start, end = end });

                download.ListData.Add(await multi.ReadFirstOrDefaultAsync<int>());
                register.ListData.Add(await multi.ReadFirstOrDefaultAsync<int>());
            }

            statistics.ItemDataList.Add(download);
            statistics.ItemDataList.Add(register);

            return statistics;
        }

        /// <summary>
        /// 获取内容管理统计数据
        /// </summary>
        /// <returns></returns>
        public async Task<StatisticsDataWeekDto> GetContentWeekDataAsync()
        {
            var weeks = GetWeeks();

            StatisticsDataWeekDto statistics = new StatisticsDataWeekDto
            {
                DataList = weeks
            };

            Dictionary<string, string> valuePairs = new Dictionary<string, string>
            {
                {"八股文","QuestionInfos" },
                {"考题","ExamQuestions" },
                {"分享","ShareInfos" },
                {"反馈","AppFeedbacks" }
            };

            foreach (var valuePair in valuePairs)
            {
                StatisticsDataDto detail = new StatisticsDataDto
                {
                    StatisticsName = valuePair.Key
                };
                foreach (var week in weeks)
                {
                    DateTime start = DateTime.Parse(week);
                    DateTime end = start.AddDays(1);

                    string sql = $"SELECT COUNT(*) FROM {valuePair.Value} WHERE CreatedTime >= @start AND CreatedTime <= @end";

                    var count = await _dapperRepository.QueryFirstOrDefaultAsync<int>(sql, new { start, end });
                    detail.ListData.Add(count);
                }

                statistics.ItemDataList.Add(detail);
            }

            return statistics;
        }


        private List<string> GetWeeks()
        {
            List<string> weeks = new List<string>();

            DateTime now = DateTime.Today;
            DateTime end = now.AddDays(-1);
            DateTime start = now.AddDays(-8);

            var day = (end - start).TotalDays;
            for (int i = 0; i < day; i++)
            {
                weeks.Add(start.AddDays(i).ToString("yyyy-MM-dd"));
            }

            return weeks;
        }
    }
}
