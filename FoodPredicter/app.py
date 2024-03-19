import os.path
import time

from flask import Flask, request, jsonify
from predict import predict

app = Flask("food-recognition")


@app.route("/predict", methods=['POST'])
def train_model():
    files = request.files
    base_path = f'predict-images/{time.time()}'
    path = f'{base_path}/sub'
    if not os.path.isdir(path):
        os.makedirs(path)
    for file in files:
        files[file].save(f'{path}/{files[file].filename}')
    result = predict(base_path, False)
    return jsonify({'result': result})


app.run(host='localhost', port=14100, debug=True)
