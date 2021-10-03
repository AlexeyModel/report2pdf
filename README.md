# API: Creating report format .pdf

Testing: csv reading, creating PDF file, generation report file HTML

Study these projects. Do not use it for professional work without first processing the code.

Folder report2pdf/Report2Pdf/resource/ contains files needed working.

- Mall_Customers.csv - info csv file.
- output.json - info example body in post method.
- index.light.html - source generation Report pdf.
- style.css - source generation Report pdf.

# Installing on Linux or macOS

<h3>Installing .NET</h3>

See this url about how to install .NET on Linux

https://docs.microsoft.com/en-us/dotnet/core/install/linux

And this url about how to install .NET on macOS

https://docs.microsoft.com/en-us/dotnet/core/install/macos

<h3>Installing Chrome</h3>

See this url about how to install Chrome on Linux

https://support.google.com/chrome/a/answer/9025903?hl=en

And this url about how to install Chrome on macOS

https://support.google.com/chrome/a/answer/7550274?hl=en

# Example installing Chrome on Linux Ubuntu

1. wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | sudo apt-key add -

2. sudo sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list'

3. sudo apt-get update

4. sudo apt-get install google-chrome-stable

5. google-chrome --version

6. google-chrome --no-sandbox --user-data-dir
