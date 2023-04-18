﻿using System;
using System.Net.Security;
using System.Security;

namespace OneIdentity.SafeguardDotNet.Authentication
{
    internal class AccessTokenAuthenticator : AuthenticatorBase
    {
        public AccessTokenAuthenticator(string networkAddress, SecureString accessToken,
            int apiVersion, bool ignoreSsl, RemoteCertificateValidationCallback validationCallback) : base(networkAddress, apiVersion, ignoreSsl, validationCallback)
        {
            if (accessToken == null)
                throw new ArgumentException("Parameter may not be null", nameof(accessToken));
            AccessToken = accessToken.Copy();
        }

        public override string Id => "AccessToken";

        protected override SecureString GetRstsTokenInternal()
        {
            throw new SafeguardDotNetException("Original authentication was with access token unable to refresh, Error: Unsupported operation");
        }

        public override object Clone()
        {
            throw new SafeguardDotNetException("Access token authenticators are not cloneable");
        }
    }
}
