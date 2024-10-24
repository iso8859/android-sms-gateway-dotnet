# android-sms-gateway-dotnet
Use Android SMS gateway from Dotnet/C# world

## Introduction
This project is a simple example on how to use Android SMS gateway from Dotnet/C# world.

## Built-in limit

Android devices have built-in SMS rate limits to prevent abuse and protect users from malicious apps sending excessive messages. Here are the key details about Android's SMS rate limits:

Default SMS Limits
- For Android versions 4.1.1 and newer: Maximum of 30 SMS messages every 30 minutes12
- For Android versions older than 4.1.1: Maximum of 100 SMS messages per hour1

These limits apply per app, not per device. If an app exceeds the limit, Android will typically show a warning prompt to the user before allowing more messages to be sent.

## Architecture

Architecture is very simple. Everything is articulated arround a SQLite database.

You can read and write new sms in the database with the library asgdotnet_lib. The library is used by the server and the client.

The service asgdotnet_service is a Windows service or console to consume the SMS from SQLite and send them to the Android device.

The web server asgdotnet_server is a simple web server to send new SMS and see when they will be sent.
