 
using System;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

// Replace with your actual subscription key and region
string subscriptionKey = "subscription_key";
string subscriptionRegion = "region";

// Function to configure speech synthesizer
SpeechConfig GetSpeechConfig()
{
    var config = SpeechConfig.FromSubscription(subscriptionKey, subscriptionRegion);
    config.SpeechSynthesisVoiceName = "en-US-AndrewMultilingualNeural";
    return config;
}

// Function to synthesize speech from text
async Task SynthesizeSpeech(string text)
{
    using (var speechSynthesizer = new SpeechSynthesizer(GetSpeechConfig()))
    {
        // Speak the provided text
        using (var audioConfig = AudioConfig.FromDefaultOutput())
        {
            SynthesisResult synthesisResult = await speechSynthesizer.SpeakTextAsync(text);

            if (synthesisResult.Reason == ResultReason.Canceled)
            {
                Console.WriteLine($"Speech canceled: {synthesisResult.Text}");
            }
            else if (synthesisResult.Reason == ResultReason.Failed)
            {
                Console.WriteLine($"Speech synthesis failed: {synthesisResult.ErrorDetails}");
            }
            else
            {
                using (var audioStream = await synthesisResult.GetAudioStreamAsync())
                {
                    await audioStream.CopyToAsync(Console.OpenStandardOutput());
                }
            }
        }
    }
}

public static void Main(string[] args)
{
    // Get user input
    Console.WriteLine("Enter the text you want to synthesize:");
    string userInput = Console.ReadLine();

    // Call function to synthesize speech with user input
    SynthesizeSpeech(userInput).Wait();
}
