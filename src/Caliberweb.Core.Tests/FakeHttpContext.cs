using System;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;

namespace Caliberweb.Core
{
    public class FakeHttpContext
    {
        public static void Create()
        {
            new FakeHttpContext();
            Thread.Sleep(500);
        }

        private FakeHttpContext()
        {
            var request = new SimpleWorkerRequest("/test", @"c:\inetpub", "test.html", null, Console.Out);
            var context = new HttpContext(request);
            var stateContainer = new HttpSessionStateContainer(Guid.NewGuid().ToString(), new SessionStateItemCollection(),
                                                                          new HttpStaticObjectsCollection(), 60000, false, HttpCookieMode.UseCookies,
                                                                          SessionStateMode.InProc, false);
            SessionStateUtility.AddHttpSessionStateToContext(context, stateContainer);
            HttpContext.Current = context;
        }
    }
}