# cryptstr

Crypting string tool.

Download here 👉 [CryptStr - nuget.org](https://www.nuget.org/packages/CryptStr/)

## How to use

### Generate Key File

First, you have to generate config file.

```
$ cryptstr gen
```

Then, config file (named ```cryptstr.json```) has generated.

Config file contents like that.👇

```
{"Key":"<Base64String>","IV":"<Base64String>"}
```

I recommend you might register to environment variable.

That would be easier to use this command.

### Encrypt String Value

```
$ cryptstr enc <plain_string_value> -k <key_of_config_file> -v <iv_of_config_file>
```

### Decrypt String Value

```
$ cryptstr dec <encrypted_string_value> -k <key_of_config_file> -v <iv_of_config_file>
```
