# Serial Communication protocol

Connection between the Controller and the Screen is a non-encrypted mutual agreement protocol.

**PROTOCOL VERSION: 00**

**LAST UPDATED: 2022/12/15**

----

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
- - Any other value is considered a deny 
- - The screen controller may deny for many resons depending on implementation
- CD is a random generated connection reference number
- - The controller must always use this in the command header to enasure that the connection is still maintained through the same two devices.

### Responses from the Display Controller
Unlike the main controller, the display only ever responds with exactly two bytes.
```
+---------+
| CONTROL |
+----+----+
| AF | 00 |    - Accept
+----+----+
| AF | FF |    - Deny (or close)    
+----+----+
etc...
```
- `00` is the connection accepted and initialized command
- `0F` is the connection denied and initialized command
- `01` is the success control response
Faliure responses:
- `02` Internal exception
- `03` Problem with the screen
- `04` Unknown command
- LENDATA is the length of additional data (in bytes)

## Sustaining a connection

There are no keep alive packages, the Controller and Screen rely on the reference number matching in command headers.

The Screen must ignore any messages sent not begining with the configured reference number.

The controller responds

## Controller commands

The list of commands is not definitive, newer versions of the protocol may add new commands.
```
+----+-----------------------+-----------------------------------------+
| 0F | Connection close      | No additional Data                      |
+----+-----------------------+-----------------------------------------+
| 11 | Full screen write     | Serial Data of new screen state         |
+----+-----------------------+-----------------------------------------+
| 12 | Pixel Write           | Location (2 bytes) state (AA on/55 off) |
+----+-----------------------+-----------------------------------------+
```

## Data structure
```
+---------+---------+----------
| CONTROL | LENDATA | DATA
+----+----+----+----+----------
| AF | 00 | 00 | 00 | null
+----+----+----+----+----------
| AF | 12 | 00 | 03 | 03 02 AA 
+----+----+----+----+----------
```
The DATA field's length is defined by the two LENDATA bytes. Due to LENDATA being limited to two bytes, only data of upto 65535 bytes (~64 KB) is possible, but that should be enough for almost all use cases and at that point, the transfer speed of the serial connection would be limiting factor.