import io
import os.path
import time
import zipfile
from base64 import encodebytes

from PIL import Image
from flask import Flask, request, send_file, jsonify
from extract import crop

app = Flask("dish-extractor")


@app.route("/extract", methods=['POST'])
def extract():
    files = request.files
    path = f'extract-images/{time.time()}'
    filename = ""
    if not os.path.isdir(path):
        os.makedirs(path)
    for file in files:
        files[file].save(f'{path}/{files[file].filename}')
        filename = files[file].filename
        break
    result_path = crop(path, filename)
    files = os.listdir(result_path)
    encoded_images = []
    for image_path in files:
        encoded_images.append(get_response_image(f"{result_path}/{image_path}"))
    return jsonify({'result': encoded_images})


def get_response_image(image_path):
    pil_img = Image.open(image_path, mode='r')  # reads the PIL image
    byte_arr = io.BytesIO()
    pil_img.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = encodebytes(byte_arr.getvalue()).decode('ascii')  # encode as base64
    return encoded_img


app.run(host='localhost', port=14200, debug=True)
