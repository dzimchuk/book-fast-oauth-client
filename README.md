# book-fast-oauth-client

A sample demonstrating OAuth2 Authorization Code Grant flow when calling [BookFast API](https://github.com/dzimchuk/book-fast-api) protected by Azure Active Directory.

## Configuration

Use environment variables, user-secrets or appsettings.json to configure the sample.

```
"Authentication": {
	"AzureAd": {
		"AuthorizationEndpoint": "",
		"TokenEndpoint": "",
		"ClientId": "",
		"ClientSecret": "",
		"Resource": "<BookFast API AppId in Azure AD>"
	}
},
"Api": {
	"BaseUrl": "<BookFast API base address, e.g. https://localhost:44361/>"
}
```
