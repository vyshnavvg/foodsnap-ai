from flask import Flask, request, jsonify
from keras.models import load_model
import numpy as np
from PIL import Image

# To Run  flask --app .\aiModelApp.py run

app = Flask(__name__)
model = load_model("food_classifier_model.h5")

def preprocess_image(image):
    image = Image.open(image).resize((224,224)) #Resize to match CNN input
    image = np.array(image) / 255.0 #normalize pixel value
    image = np.expand_dims(image, axis=0)
    return image

@app.route('/detect-image', methods=['POST'])
def detectImage():
    file = request.files['image']
    processed_image = preprocess_image(file)

    prediction = model.predict(processed_image)
    food_classes = ['Apple', 'Banana', 'Mango']
   
    predicted_index = np.argmax(prediction)
    predicted_class = food_classes[predicted_index]
    confidence_score = float(prediction[0][predicted_index] * 100)

    # convet full prediction into dict
    prediction_dict = {food_classes[i]: round(float(prediction[0][i])* 100, 2) for i in range(len(food_classes))}

    return jsonify({
        "prediction": predicted_class,
        "confidence": round(confidence_score, 2), #rounded to 2 decimal places
        "fullPrediction": prediction_dict #show all
        })

@app.route("/")
def hello_world():
    return "<h1>Hello, World from Vyshnav</h1>"

if __name__ == '__main__':
    app.run(port=5000, debug=True)
