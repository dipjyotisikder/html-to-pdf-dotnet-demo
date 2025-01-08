# HTML to PDF .NET Demo

A **.NET 8** application for generating PDF documents using the **DinkToPdf** and **PdfSharpCore** libraries. It provides functionality to create single and chunked PDFs from dynamic data.

## ✨ Features

- 📄 Generate a single PDF document from dynamic data.
- 📚 Generate chunked PDFs and merge them into a single document.
- 🛠️ Customizable HTML table generation for PDF content.
- 📑 Supports pagination for large datasets.

## 📋 Requirements

- .NET 8
- DinkToPdf
- PdfSharpCore

## ⚙️ Installation

1. **Clone the repository:**
2. **Navigate to the project directory:**
3. **Restore the dependencies:**
## 🚀 Usage

### Create a Single PDF

To create a single PDF, use the `CreatePDF` method:

### Create a Chunked PDF

To create a chunked PDF, use the `CreateChunkedPDF` method:

## 🎨 Customization

### Modify Data

You can modify the data used for generating the PDF by updating the `GetData` method in `PdfMaker.cs`.

### Customize HTML Table

To customize the HTML table structure, update the `GenerateTable` method in `PdfMaker.cs`.

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## 📄 License

This project is licensed under the MIT License.
