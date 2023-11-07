<Query Kind="Program">
  <NuGetReference>Rock.Core.Newtonsoft</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


void Main()
{
    // Your OpenAI API key
    var apiKey = "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";
    
    // The text you want to convert to speech
    var text = "Your text to convert to speech";

    // The URI of the OpenAI text-to-speech endpoint
    var uri = "https://api.openai.com/v1/audio/speech"; 

    // Set up the HTTP client with the necessary headers
    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        // Create the request content
		var json = new { 
			model= "tts-1",
    		input= "Today is a wonderful day to build something people love!",
    		voice= "onyx" 
		};
	
        var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

		// Post the request and get the response
        var response = client.PostAsync(uri, content).Result;

        // Ensure we got a successful response
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error: " + response.StatusCode);
        }
		
        // Post the request and get the response
        var responseBytes = response.Content.ReadAsByteArrayAsync().Result;

		// Specify the file path for the MP3 file
        var filePath = @"C:\Users\Bill Sithiro\Downloads\audio.mp3";

        // Write the byte array to the file
        File.WriteAllBytes(filePath, responseBytes);

        // Output the file path
        $"File saved to: {filePath}".Dump();
    }
}