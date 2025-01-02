using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Util.Ext;

namespace System
{

 

    public static class JWTHelper
    {
        static IJwtAlgorithm algorithm = new HMACSHA256Algorithm();//HMACSHA256加密
        static IJsonSerializer serializer = new JsonNetSerializer();//序列化和反序列
        static IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//Base64编解码
        static IDateTimeProvider provider = new UtcDateTimeProvider();//UTC时间获取
        const string secret = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC4aKpVo2OHXPwb1R7duLgg";//服务端


        public static string CreateJWT(Dictionary<string, object> payload)
        {          
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, secret);
        }
        public static bool ValidateJWT(string token, out string payload, out string message)
        {
            bool isValidted = false;
            payload = "";
            try
            {
                IJwtValidator validator = new JwtValidator(serializer, provider);//用于验证JWT的类

                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);//用于解析JWT的类
     
                payload = decoder.Decode(token, secret, verify: true);

                isValidted = true;

                message = "";
            }
            catch (TokenExpiredException)//当前时间大于负载过期时间（负荷中的exp），会引发Token过期异常
            {
                message = "过期了！";
            }
            catch (SignatureVerificationException)//如果签名不匹配，引发签名验证异常
            {
                message = "签名错误！";
            }
            return isValidted;
        }


        public static string GetJWT(object data,int min= 60*24*30*3)
        {
            var payload = new Dictionary<string, object>
            {
                { "iss","hz"},//发行人
                { "exp",DateTime.Now.AddMinutes(min).ToTimestamp().ToString() },//到期时间
                { "sub", "webtoken" }, //主题
                { "aud", "own" }, //用户
                { "iat", DateTime.Now.ToString() }, //发布时间 
                { "data" ,data.ToJson()}
            };
            var jwt = CreateJWT(payload);
            return jwt;
        }
        public static T ParseJWT<T>(string jwt,out string error) where T:class 
        {
            string content;        
            var res = ValidateJWT(jwt, out content, out error);
            if (res)
            {
                var data= content.ToObject<Dictionary<string, object>>()["data"];
                return data.ToString().JsonToObject<T>();
            }
            else {
                return default(T);
            }
        }


    }
}
