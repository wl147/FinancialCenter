using FC.Base.ExceptionHandler;
using FC.Base.OperationBase;
using FC.DAL;
using FC.DAL.EFBase;
using FC.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.BLL.Bussiness
{
    public class LeagueMatchesBusiness:BusinessBase<FC_LeagueMatches,LeagueMatchesRepository>
    {
        LeagueMatchesRepository repository = new LeagueMatchesRepository();
        /// <summary>
        /// 新增联赛信息
        /// </summary>
        /// <returns></returns>
        public StatusResult CreateLeagueMatch(string leagueCode,string leagueName,string imageUrl,int teamCount)
        {
            StatusResult result = new StatusResult();
            FC_LeagueMatches leagueMatch = new FC_LeagueMatches();
            leagueMatch.LeagueCode = leagueCode;
            leagueMatch.LeagueName = leagueName;
            leagueMatch.ImageUrl = imageUrl;
            leagueMatch.TeamCount = teamCount;
            try
            {
                repository.Create(leagueMatch);
            }
            catch(Exception ex)
            {
                ExceptionHandler.LogExcetion(ex);
                result.Status = Status.Error;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
