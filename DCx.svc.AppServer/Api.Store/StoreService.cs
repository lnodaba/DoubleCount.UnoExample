using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//using DCx.Config;
//using DCx.Data;
//using DCx.Defines;
//using DCx.Messaging;
//using DCx.Service;
//using DCx.Store;

namespace DCx.AppServer
{
    public class StoreService : IStoreService
    {
        #region members / properties

        //private readonly    IAppService     m_appService    = null;

        //private readonly    Guid            m_cfgServerId   = Guid.Empty;
        //private             StoreList       StoreList       =>  this.m_appService.StoreList;

        #endregion

        #region ctors
        public StoreService(/*IAppService appService*/)
        {
            //this.m_appService   =   appService;
            //this.m_cfgServerId  =   this.m_appService.CfgAppServer.ServerId;
        }

        #endregion

        #region Method - GetStoresList

        //public MsgBagStore GetStoresList (byte[] rawData)
        //{
        //    var msgBagStore = new MsgBagStore(EMsgContent.StoreListResponse);

        //        msgBagStore.ServerId        = this.m_cfgServerId;
        //        msgBagStore.EnterpriseId    = Guid.Empty;
        //        msgBagStore.StoreInfos      = this.StoreList.ListCustomers;

        //    return msgBagStore;
        //}
        #endregion
    }
}
