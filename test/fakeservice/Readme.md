How to execute the fakeservice


Make sure you have python and Flask

```bash
python fakepassportservice.py
```

To make the passport scanner works you need to call in this order:
- http://localhost:12212/initialise
- http://localhost:12212/readdocument
- http://localhost:12212/shutdown

For QRcode it is:
- http://localhost:12212/initialise
- http://localhost:12212/qrcode
- http://localhost:12212/shutdown


if you need the APIs to return error, you can use:
- http://localhost:12212/seterror
- http://localhost:12212/unseterror

