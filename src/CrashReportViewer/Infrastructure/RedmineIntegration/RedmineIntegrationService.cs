using System;
using System.IO;
using System.Net;
using CrashReport.Domain;
using Newtonsoft.Json;

namespace CrashReportViewer.Infrastructure.RedmineIntegration
{
	public class RedmineIntegrationService: IRedmineIntegrationService
	{
		private const string IssueCreate = "{0}/projects/{1}/issues.json?key={2}";
		private const string ViewIssue = "{0}/issues/{1}";

		private readonly string _apiKey;
		private readonly string _redmineUrl;
		private int _trackerId = 19;

		public RedmineIntegrationService(string apiKey, string redmineUrl)
		{
			_apiKey = apiKey;
			_redmineUrl = redmineUrl;
		}

		public string CreateIssue(Problem problem, string projectId, string problemUrl)
		{
			var issue = TransalteProblemToIssue(problem, problemUrl, projectId);
			var issueContainer = new IssueContainer(issue);
			var serializer = new JsonSerializer();

			var webRequest = WebRequest.Create(string.Format(IssueCreate, _redmineUrl, projectId, _apiKey));
			webRequest.Method = WebRequestMethods.Http.Post;
			webRequest.ContentType = "application/json";

			using (var requestStream = webRequest.GetRequestStream())
			{
				using (var writer = new StreamWriter(requestStream))
				{
					serializer.Serialize(writer, issueContainer);
				}
			}

			var response = webRequest.GetResponse();
			using (var responseStream = response.GetResponseStream())
			{
				if(responseStream == null)
					throw new InvalidOperationException("Failed to get response");

				using (var reader = new StreamReader(responseStream))
				{
					using (var jsonReader = new JsonTextReader(reader))
					{
						issueContainer = serializer.Deserialize<IssueContainer>(jsonReader);
					}
				}
			}

			return string.Format(ViewIssue, _redmineUrl, issueContainer.issue.id);
		}

		private Issue TransalteProblemToIssue(Problem problem, string problemUrl, string projectId)
		{
			var description = $"{problem.Description}\r\n{problemUrl}";

			return new Issue
			{
				project_id = projectId,
				tracker_id = _trackerId,
				assigned_to_id = null,
				subject = problem.ShortDescription,
				description = description
			};
		}
	}
}