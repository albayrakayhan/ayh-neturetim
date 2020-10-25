using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using aceka.infrastructure.Repositories;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;
using aceka.web_api.Models.KurModels;
using System.Web.Http.Cors;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Projenin menüsüne ait metodlar
    /// </summary>
    public class MenuController : ApiController
    {
        #region Degiskenler
        private List<MenuItem> menuler = null;
        private MenuRepository menuRepository = null;
        #endregion

        // GET: api/Menu
        /// <summary>
        /// Projede kullanılan menu listesini verir.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MenuItem> Get()
        {
            menuRepository = new MenuRepository();
            menuler =menuRepository.Getir();
            return menuler;
        }
        
        //// GET: api/Menu/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Menu
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Menu/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Menu/5
        //public void Delete(int id)
        //{
        //}
    }
}
