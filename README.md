# CRUD with AI UI

A modern Blazor WebAssembly application that provides an AI-powered interface for managing products through natural language conversation. Built with .NET 10.0 and powered by Claude AI's tool-calling capabilities.

API backend application (https://github.com/alexholmes4/crud-with-ai-api)

## Features

- **AI-Powered Chat Interface**: Interact with an intelligent assistant to perform product operations through natural language
- **Product Management**: Browse, view, and manage products with a clean, responsive interface
- **Real-time Feedback**: Action buttons in chat responses for quick navigation and product operations
- **Error Handling**: Comprehensive error handling with automatic retry logic and user-friendly notifications
- **Modern UI**: Built with Blazor components and Bootstrap 5 styling

## Technology Stack

- **Frontend**: Blazor WebAssembly with .NET 10.0
- **UI Components**: Bootstrap 5 with custom Blazor components
- **Notifications**: Blazored.Toast v4.1.0
- **HTTP Client**: HttpClient with retry logic and configuration-based settings
- **AI Backend**: Claude AI integration via tool-calling API
- **JSON Serialization**: System.Text.Json with Web defaults

## Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- A backend API configured with Claude AI integration
- API endpoint URL configuration

## Architecture

The application follows a service-based architecture with:
- **Dependency Injection**: Configured in Program.cs
- **Component Composition**: Modular Razor components
- **Separation of Concerns**: Business logic in services, UI in components
- **Error Handling**: Centralized via extension methods and API service base class

## Future Enhancements

- [ ] User authentication and authorization
- [ ] Product filtering and search
- [ ] Conversation history persistence
- [ ] Advanced chat features (file uploads, voice input)
- [ ] Multi-language support

## License

MIT License

## Acknowledgments
- Part of this README was generated with AI.
- [Anthropic SDK](https://github.com/tghamm/Anthropic.SDK) - C# SDK for Anthropic's Claude API
- Claude 4.5 Haiku - The AI model powering conversational interactions
