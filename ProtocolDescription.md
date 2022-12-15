# Serial Communication protocol

Connection between the Controller and the Screen is a non-encrypted mutual agreement protocol.

PROTOCOL VERSION: 00

LAST UPDATED: 2022/12/15

## Initializing a connection
### Controller begin message
```
+-------------------+----+----+----+----+
|    MAGIC HEADER   | VE | CN | Wd | He |
+----+----+----+----+----+----+----+----+
| AA | 55 | AA | 55 | 00 | 07 | 18 | 07 |
+----+----+----+----+----+----+----+----+
```

Where:
- Magic header is the header of `AA 55 AA 55` to signal the beginning of the connection message (easily notice in potential noise)
- VE is Protocol version
- - Backwards compatibility from the controller is expected, but if the connected display has a newer version, connection should be refused
- CN is the Count of modules expected
- Wd and He is the Width and Height of the expected modules

### Acknowledge from screen
```
+----+----+----+----+----+----+
| VE | CN | Wd | He | AC | CD |
+----+----+----+----+----+----+
| 00 | 07 | 18 | 07 | FF | AF |
+----+----+----+----+----+----+
```
- AC is the accept flag: 
- - `FF` for Accept
- - Any other value is considered as a deny 
- - The screen controller may deny for many resons depending on implementation
- CD is a random generated connection reference number
- - The controller must always use this in the command header to enasure that the connection is still maintained through the same two devices.

### Connection accept/deny from Controller
```
+---------+---------+
| CONTROL | LENDATA |
+----+----+----+----+
| AF | 00 | 00 | 00 |    - Accept
+----+----+----+----+
| AF | FF | 00 | 00 |    - Deny (or close)
+----+----+----+----+
```
- `00` is the connection accepted and initialized command
- `FF` is the connection accepted and initialized command
- LENDATA is the length of additional data (in bytes)

## Sustaining a connection

There are no keep alive packages, the Controller and Screen rely on the reference number matching in command headers.

The Screen must ignore any messages sent not begining with the configured reference number.

## Commands

The list of commands is not definitive, newer versions of the protocol may add new commands.
```
+----+---------------------------+-----------------------------------------+
| 00 | Connection accepted       | No additional Data                      |
+----+---------------------------+-----------------------------------------+
| FF | Connection refused/closed | No additional Data                      |
+----+---------------------------+-----------------------------------------+
| 11 | Full screen write         | Serial Data of new screen state         |
+----+---------------------------+-----------------------------------------+
| 12 | Pixel Write               | Location (2 bytes) state (AA on/55 off) |
+----+---------------------------+-----------------------------------------+
```