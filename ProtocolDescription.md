# Serial Communication protocol

The connection between the Controller and the Screen is a non-encrypted mutual agreement protocol. There are no specified serial settings; baud rate, and other available settings left for the implementation to decide based on their needs.

**PROTOCOL VERSION: 01**

**LAST UPDATED: 2023/08/0**

----

## Initializing a connection
### Controller connection request
> :information_source: Note: All numbers shown on this page are HEX.
```
+-------------------+----+----+----+----+
|    MAGIC HEADER   | VE | CN | Wd | He |
+----+----+----+----+----+----+----+----+
| AA | 55 | AA | 55 | 01 | 01 | 18 | 07 |
+----+----+----+----+----+----+----+----+
```

Where:
- Magic header is the header of `AA 55 AA 55` to signal the beginning of the connection message (easily noticed in potential noise)
- VE is the protocol version
  - Backwards compatibility from the controller is generally expected, but if the connected display has a newer version, the connection should be refused.
- CN is the Count of modules expected
- Wd and He is the Width and Height of the expected modules

### Acknowledge from the screen
The screen (assuming proper function and non-connected state) will respond with the following message(s) to any connection request:
```
+----+----+----+----+----+----+
| VE | CN | Wd | He | AC | CD |
+----+----+----+----+----+----+
| 01 | 01 | 18 | 07 | FF | AF |
+----+----+----+----+----+----+
```
- AC is the accept flag: 
  - `FF` for Accept
  - Any other value is considered a deny 
  - The screen controller may deny for many reasons depending on the implementation
- CD is a randomly generated connection reference number
  - The controller must always use this in the command header to ensure that the connection is still maintained through the same two devices. 

## Sustaining a connection

There are no keep-alive packages. The Controller and Screen rely on the reference number matching in command headers.

The Screen must ignore any messages not beginning with the configured reference number.

The controller responds to every command with two bytes.

## Controller commands

The list of commands is not definitive. Newer versions of the protocol may add new commands.
```
+----+-----------------------+-----------------------------------------+
| 0F | Connection close      | No additional Data                      |
+----+-----------------------+-----------------------------------------+
| 11 | Full screen write     | Serial Data of new screen state         |
+----+-----------------------+-----------------------------------------+
| 12 | Pixel Write           | Location (2 bytes) state (01 on/00 off) |
+----+-----------------------+-----------------------------------------+
| 19 | Force screen write    | Serial Data of new screen state         |
+----+-----------------------+-----------------------------------------+
```

## Data structure
```
+---------+---------+----------
| HEADER  | LENDATA | DATA
+----+----+----+----+----------
| AF | 00 | 00 | 00 | null
+----+----+----+----+----------
| AF | 12 | 00 | 03 | 03 02 01 
+----+----+----+----+----------
```
The header is two bytes long and is made up of the Connection reference number and the command

The DATA field's length is defined by the two LENDATA bytes. Due to LENDATA being limited to two bytes, only data of up to 65535 bytes (~64 KB) is possible, but that should be enough for almost all use cases, and at that point, the transfer speed of the serial connection would be the limiting factor.

> :information_source: Planned update: LENDATA may be removed in the future as the length of the data is obvious from the command.

### Responses from the Display Controller
Unlike the main controller, the display only responds with exactly two bytes after every command once a connection is established.
```
+---------+
| CONTROL |
+----+----+
| AF | 01 |    - Success (usually signals the completion of a screen write)
+----+----+
| AF | 05 |    - Bad data (usually sent when data is less than expected
+----+----+
etc...
```
- `FF` is the connection accepted and initialized command
- `01` is the success control response

Failure responses:
- `02` Internal exception
- `03` Problem with the screen
- `04` Unknown command
- `05` Bad data
