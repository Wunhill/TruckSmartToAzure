using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using TruckSmartManifestTypes;
using TruckSmartWeb.Models;

namespace TruckSmartWeb.Controllers
{
    public class ManifestController : ApiController
    {


        // POST: api/Manifest
        public ManifestResponse Post(ManifestRequest datagram)
        {
            var request = Request;
            //Append the captured client IP to the datagram.  
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                datagram.ClientAddress = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                datagram.ClientAddress = prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                datagram.ClientAddress = HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                datagram.ClientAddress = null;
            }
            return ManifestModel.ProcessMessage(datagram);
        }


    }
}
