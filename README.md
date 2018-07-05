# NOnStar
Unofficial C# based OnStar client for locking/unlocking starting/stopping OnStar enabled vehicles (with active subscription).

Originally written a few years back after a weekend of reverse engineering, it sat on a shelf for a while as I could not identify the secret values used to encrypt the JWT. Fast-foward a bit and @mikenemat did, and published a Python client: https://github.com/mikenemat/gm-onstar-probe

Now, plugging those values (which appear to be changed periodically) allows my code to work as well, opening up all sorts of possibilities like triggering it from a custom Alexa (or other personal assistant) skill, to a console app on your desktop.

Requirements:
* .NET Core 2.0
* An vehical with an OnStar subscription
* An OnStar account

This code has only been tested on a 2011 Chevy Equinox.

# Use at your own risk. I take no responsibility should your account be terminated.
