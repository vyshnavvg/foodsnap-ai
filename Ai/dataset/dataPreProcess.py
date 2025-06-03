import cv2
import os
import tensorflow as tf

folderList = ["apple", "banana", "mango"]

for folder in folderList:
    for fileName in os.listdir(f"dataset/{folder}"):
        image = cv2.imread(f"dataset/{folder}/{fileName}")
        resized = cv2.resize(image, (224, 224)) #Resize to match CNN input

        cv2.imwrite(f"dataset/{folder}/{fileName}", resized)


