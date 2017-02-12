# libtcod-net 

libtcod-net: Beautiful C# wrappers around the roguelike library [libtcod] 

## Installation

1. Build the project.
2. Copy the managed and native libraries in dist to your project to consume
3. On macOS inside your Xamarin.Mac project you will need to add the following to your Additional MMP arguments, updating YOUR_PATH to point to your specific location.
	
--native-reference=YOUR_PATH/external/macos/libtcod.a --native-reference=YOUR_PATH/external/macos/libSDL-1.2.0.dylib --link_flags="-framework OpenGL" --native-reference=YOUR_PATH/external/macos/CustomSDLMain.a

This is to work around https://bugzilla.xamarin.com/show_bug.cgi?id=51693 for now.

## Usage

See test code included and native [documentation] 

## TODO 

- Random
- System

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## History

- Was originally hand created bindings - https://code.google.com/archive/p/libtcod-net/
- Then for awhile was swig generated bindings inside libtcode
- Now modernized hand created bindings

## Credits

- Chris Hamons for C# bindings
- Jice & Mingos & rmtew and the rest of the libtcod test for the native library

## License

MIT

[libtcod]: https://bitbucket.org/libtcod/libtcod
[documentation]: http://roguecentral.org/doryen/data/libtcod/doc/1.5.1/index2.html?c=true&cpp=false&cs=false&py=false&lua=false
