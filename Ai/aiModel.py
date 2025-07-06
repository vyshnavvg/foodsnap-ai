import keras
from keras.models import Sequential
from keras.layers import Conv2D, MaxPooling2D, Flatten, Dense, Dropout
#from keras.preprocessing.image import ImageDataGenerator
from keras.src.legacy.preprocessing.image import ImageDataGenerator
from keras.layers import GlobalAveragePooling2D
import matplotlib.pyplot as plt

 # Food Snap AI Model
 
model = Sequential([

    # Conv2D extracts features like edges, texttures, colors of fruits and its patterns
    Conv2D(32, (3,3), activation="relu", input_shape=(244,244,3)),
    # Pooling layer used to reduce dimenstions of feature map from previous layers before passing to next layer, Make computation faster while keeping relevant info
    MaxPooling2D(pool_size=(2,2)),

    Conv2D(64, (3,3), activation='relu'),
    MaxPooling2D(pool_size=(2,2)),

    Conv2D(128, (3,3), activation='relu'),
    MaxPooling2D(pool_size=(2,2)),

    GlobalAveragePooling2D(),
    # converts 2D feature maps into a single vector for classification
    Flatten(),

    Dense(128, activation='relu'),
    # prevents overfitting, so model generalizes better.
    Dropout(0.5), # Reduces overfitting
    # Uses Softmax for multi-class classification
    Dense(3, activation="softmax") # 3 output classes (Apple, Banana, Mango)
])

model.compile(
    optimizer='adam',
    loss='categorical_crossentropy',
    metrics=['accuracy']
)

#Data Augmentation
train_datagen = ImageDataGenerator(
    rescale =1./255, #Normalize pixel values (0 to 1 instead of 0 to 255)
    rotation_range = 20, # Randomly rotate image by up to 20 degress
    width_shift_range = 0.2, # Shift image width randomly by 20% (left or right)
    height_shift_range = 0.2, # Shift image height randomly by 20% (up or down)
    horizontal_flip = True, # Flip images randomly (left-right)
    validation_split = 0.2 # reserve 20% data for validation
)

#Loading Images from folder
train_generator = train_datagen.flow_from_directory(
    'dataset/', 
    target_size = (224, 224),
    batch_size = 32,
    class_mode = 'categorical', #Automatically assigns labels based on folder names
    subset = 'training' #uses 80% images for training
)

# Preparing Validation Data
val_generator = train_datagen.flow_from_directory(
    'dataset/',
    target_size = (244, 244),
    batch_size = 32,
    class_mode = 'categorical',
    subset = 'validation'
)

#Start CNN training
history = model.fit(
    train_generator,
    validation_data= val_generator,
    epochs= 30
)

# Elavuate & Visualize results
plt.plot(history.history['accuracy'], label='Training Accuracy')
plt.plot(history.history['val_accuracy'], label='Validation Accuracy')
plt.xlabel('Epochs'),
plt.ylabel('Accuracy')
plt.legend()
plt.show()

# Save model
model.save("food_classifier_model_v2.h5")