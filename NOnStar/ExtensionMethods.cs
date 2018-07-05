using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar
{
    static class ExtensionMethods
    {
        public static JwtBuilder AddClaims(this JwtBuilder builder, DeviceAuth deviceAuth)
        {
            return builder
            .AddClaim("client_id", deviceAuth.client_id)
            .AddClaim("device_id", deviceAuth.device_id)
            .AddClaim("username", deviceAuth.username)
            .AddClaim("password", deviceAuth.password)
            .AddClaim("nonce", deviceAuth.nonce)
            .AddClaim("timestamp", deviceAuth.timestamp)
            .AddClaim("scope", deviceAuth.scope)
            .AddClaim("grant_type", deviceAuth.grant_type);
        }

        public static JwtBuilder AddClaims(this JwtBuilder builder, PinAuth pinPayload)
        {
            return builder
            .AddClaim("client_id", pinPayload.client_id)
            .AddClaim("device_id", pinPayload.device_id)
            .AddClaim("credential", pinPayload.credential)
            .AddClaim("credential_type", pinPayload.credential_type)
            .AddClaim("nonce", pinPayload.nonce)
            .AddClaim("timestamp", pinPayload.timestamp)
            .AddClaim("scope", pinPayload.scope);
        }
    }
}
