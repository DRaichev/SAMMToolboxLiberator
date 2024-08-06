This is a simple tool to liberate data from Excel and write it to JSON.
The JSON output is formatted based on the SAMM format specified in this [proposal](https://github.com/DRaichev/SAMM-Assessment-Format).

## Usage

Either build the project or download the binary from the [releases page](https://github.com/DRaichev/ToolboxLiberator/releases).

Then use the following command:
```
liberate <inputFile> <outputFile>
```

Example:

```bash
./ToolboxLiberator liberate SAMM_spreadsheet.xlsx output.samm
```