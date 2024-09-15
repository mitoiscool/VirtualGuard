<div align="center">

<img src="Deps/shield.png">

# VirtualGuard

This is the obfuscation engine for a project I was developing called VirtualGuard.

[Foreword](#foreword) •
[Features](#features) •
[Usage](#usage) •
[License](#license) •
[Credits](#credits)


[![forthebadge](https://forthebadge.com/images/badges/license-mit.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/powered-by-black-magic.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)

</div>

## Foreword

I once intended to sell VirtualGuard, however as soon as money gets involved with things I tend to lose interest. So here's the source code!

I am open-sourcing this months after I originally wrote it, so I may have trouble providing support on much of it. With that said, I have comments all over the code. I hope people can learn off of this! ( and see in the end that VirtualGuard was not a confuserEx clone :( )

## Features

 - Automatic hashing of constants in equality checks
 - No dispatcher - each handler has a unique offset which is then added to a fixup byte to overflow to the next handler.
 - No conditional jump opcodes through the use of arithmetic to calculate the proper jump address
 - Multi-VM Support
 - Unique virtual machines (huge list of constants is randomized on compile-time)
 - And probably a bunch more that I am forgetting.


## Usage

 - I believe that the CLI will work, and it has brief provided documentation within it. You will need to pass in the arguments to generate a configuration first, and then once you have a config file you should be able to virtualize your files with it. If worst comes to worse, just use the code in the CLI as reference for interacting with the proper engine.


## License

This repository uses the MIT license. 


## Credits

- Washi for [AsmResolver](https://github.com/Washi1337/AsmResolver)
- [KoiVM](https://github.com/yck1509/KoiVM). Love the way KoiVM is organized. Structurally very inspired by it. Not a KoiVM clone. (however impl of the abstracted handlers I will admit is a little silly)
- Echo
