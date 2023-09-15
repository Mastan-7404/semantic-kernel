// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Security;
internal sealed class BlackListService
{
    private BlackList blackList;
    public BlackListService() {
        LoadBlackList();
    }

    public bool isBlackListed(string url)
    {
        return blackList.Contains(url);
    }

    private void LoadBlackList()
    {
        using (StreamReader r = new StreamReader("C:/Users/svali/hackathon/defender-for-copilot/semantic-kernel/dotnet/samples/ConsoleApp/black-list.json"))
        {
            string json = r.ReadToEnd();
            blackList = JsonSerializer.Deserialize<BlackList>(json);
        }
    }

    private class BlackList
    {
        public List<string> urls { get; set; }
        public bool Contains(string url)
        {
            return urls.Contains(url);
        }
    }
}
