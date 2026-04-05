using System.Collections.Immutable;
using Yarp.ReverseProxy.Configuration;
using static System.Net.WebRequestMethods;

namespace ApiGateway
{
    public static class YARPConfig
    {
        public static IReadOnlyList<RouteConfig> Routes = new List<RouteConfig>()
        {
            new RouteConfig()
            {
                RouteId="Login",
                ClusterId="User",
                Match=new RouteMatch()
                {
                    Path ="/User/Login",
                    Methods=new List<string>() { "Post" },
                }
            },
            new RouteConfig()
            {
                RouteId="Register",
                ClusterId="User",
                Match=new RouteMatch()
                {
                    Path ="/User/Register",
                    Methods=new List<string>() { "Post" },
                }
            },
            new RouteConfig()
            {
                RouteId="GetProfile",
                ClusterId="User",
                Match=new RouteMatch()
                {
                    Path ="/User/GetProfile",
                    Methods=new List<string>() { "Get" },
                }
            },
            new RouteConfig()
            {
                RouteId="GetProfiles",
                ClusterId="User",
                Match=new RouteMatch()
                {
                    Path ="/User/GetProfiles",
                    Methods=new List<string>() { "Get" },
                }
            },
            new RouteConfig()
            {
                RouteId="ChangeProfile",
                ClusterId="User",
                Match=new RouteMatch()
                {
                    Path ="/User/ChangeProfile",
                    Methods=new List<string>() { "Put" },
                }
            },
            new RouteConfig()
            {
                RouteId="CreatePost",
                ClusterId="TextPost",
                Match=new RouteMatch()
                {
                    Path ="/TextPost/CreatePost",
                    Methods=new List<string>() { "Post" },
                },
                AuthorizationPolicy="Bearer"
            },
            new RouteConfig()
            {
                RouteId="UpdatePost",
                ClusterId="TextPost",
                Match=new RouteMatch()
                {
                    Path ="/TextPost/UpdatePost",
                    Methods=new List<string>() { "Put" },
                },
                AuthorizationPolicy="Bearer"
            },
            new RouteConfig()
            {
                RouteId="DeletePost",
                ClusterId="TextPost",
                Match=new RouteMatch()
                {
                    Path ="/TextPost/DeletePost",
                    Methods=new List<string>() { "Delete" },
                },
                AuthorizationPolicy="Bearer"
            },
            new RouteConfig()
            {
                RouteId="GetPost",
                ClusterId="TextPost",
                Match=new RouteMatch()
                {
                    Path ="/TextPost/GetPost",
                    Methods=new List<string>() { "Get" },
                },
                AuthorizationPolicy="Bearer"
            },
            new RouteConfig()
            {
                RouteId="GetUserPostsPage",
                ClusterId="TextPost",
                Match=new RouteMatch()
                {
                    Path ="/TextPost/GetUserPostsPage",
                    Methods=new List<string>() { "Get" },
                },
                AuthorizationPolicy="Bearer"
            },
        };
        public static IReadOnlyList<ClusterConfig> Clusters = new List<ClusterConfig>()
        {
            new ClusterConfig()
            {
                ClusterId="User",
                Destinations=new Dictionary<string, DestinationConfig>()
                {
                    {
                        "destination1",new DestinationConfig()
                        {
                            Address="http://localhost:5034"
                        }
                    }
                }
            },
            new ClusterConfig()
            {
                ClusterId="TextPost",
                Destinations=new Dictionary<string, DestinationConfig>()
                {
                    {
                        "destination2",new DestinationConfig()
                        {
                            Address="http://localhost:5164"
                        }
                    }
                }
            },
        };
    }
}
