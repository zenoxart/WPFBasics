# 🌟 WPFBasics

🚀 **WPFBasics Highlights**  
WPFBasics integrates seamlessly with **Microsoft's Fluent Style Sheet** 🎨 to deliver a modern and polished user interface. Additionally, it includes the **WPFBasics Library** 📚, a robust collection of utilities and components designed to accelerate WPF application development.

### ✨ Key Features:
- 🎨 **Fluent Style Sheet Integration**: Achieve a sleek and professional UI with Microsoft's Fluent Design principles.  
- 📚 **WPFBasics Library**: A comprehensive library of tools and components to simplify your development workflow.  

---

## 📖 Overview

WPFBasics is a WPF-based application framework designed to simplify the development of modern Windows Presentation Foundation (WPF) applications. It provides a collection of reusable components, MVVM support, and utility services to streamline application development.

---

## ⚙️ Features

- 🏗️ **MVVM Support**: Includes base classes like `ViewModelNotifyPropertyBase` to simplify property change notifications.
- 🖼️ **Custom Controls**: Provides reusable controls like `FluentGrid` with built-in dependency properties for data binding.
- 🎨 **Theming**: Includes a `FluentExtension` theme for consistent UI styling.
- 🛠️ **Utility Services**: Offers services like `DialogService`, `FileDialogService`, and `ClipboardService` for common application tasks.
- 🧩 **Command Handling**: Supports command-based interactions for better separation of concerns.
- 🔄 **Threading Utilities**: Includes helpers like `DispatcherHelper` for managing UI thread operations.

---

## 🗂️ Project Structure

The project is organized into the following directories:

- **📁 WPFBasics**: Contains the core framework components.
  - `📂 Common`: Includes shared utilities, services, and MVVM support classes.
  - `📂 Controls`: Custom WPF controls like `FluentGrid`.
  - `📂 Themes`: XAML-based themes for consistent UI styling.
- **📁 UI**: The main application project that references `WPFBasics` and demonstrates its usage.

---

## 🚀 Getting Started

### ✅ Prerequisites

- 🖥️ .NET 10.0 SDK or later  
- 🛠️ Visual Studio 2022 or Visual Studio Code with C# extensions  

### 🛠️ Build and Run

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd WPFBasics/src
