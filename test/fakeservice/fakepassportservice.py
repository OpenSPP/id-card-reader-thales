import random

from flask import Flask, jsonify, request
from samples import *

app = Flask(__name__)
bReturnError = False

@app.route('/initialise', methods=['GET', 'OPTIONS'])
def initialise():
    response = jsonify({
        'initialise': True,
    })

    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = 0

    return response

@app.route('/shutdown', methods=['GET', 'OPTIONS'])
def shutdown():
    response = jsonify({
        'shutdown': True,
    })

    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = 0

    return response

@app.route('/readdocument', methods=['GET', 'OPTIONS'])
def readdocument():
    global bReturnError
    if (bReturnError):
        response = jsonify({
            "error":  random.choice(("An error occurred reading the document - ERROR_NOT_INITIALISED",
                                     "An error occurred reading the document - ERROR_NO_DOC_ON_WINDOW",
                                     "Non-negative number required. (Parameter 'year')\\r\\nActual value was -1."))
        })
    else:
        response = jsonify({
            "given_name": random.choice(("JOHN ALBERT","JANE ALICIA")),
            "document_number": random.choice(("13TG45678", "11TT33333")),
            "photo": idphoto,
            "image": passport,
            "address": "56 RAINBOW ROAD 19999 BANGKOK THA THAÏLANDE",
            "expiry_date": "2029-12-12",
            "family_name": random.choice(("DOE","DUPONT")),
            "name": random.choice(("DOE JOHN ALBERT","DUPONT JANE ALICIA")),
            "birth_place_city": "PARIS",
            "document_type": "PASSPORT",
            "birth_place_country": "FRANCE",
            "gender": "Male",
            "birth_date": "1989-03-12"
        })

    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = 0

    return response

@app.route('/qrcode', methods=['GET', 'OPTIONS'])
def qrcode():
    global bReturnError
    if (bReturnError):
        response = jsonify({
            "error":  "No QR code detected"
        })
    else:
        response = jsonify({
            "qrcode":"11010232123"
        })

    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = 0

    return response

@app.route('/seterror', methods=['GET', 'OPTIONS'])
def seterror():
    global bReturnError
    bReturnError = True

    response = jsonify({
        "Value":"APIs will now return errors"
    })

    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = 0

    return response

@app.route('/unseterror', methods=['GET', 'OPTIONS'])
def unseterror():
    global bReturnError
    bReturnError = False

    response = jsonify({
         "Value":"APIs will now be valid"
    })

    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Cache-Control'] = 'no-cache, no-store, must-revalidate'
    response.headers['Pragma'] = 'no-cache'
    response.headers['Expires'] = 0

    return response


if __name__ == "__main__":
    app.run(host='0.0.0.0', port=12212, debug=True)
