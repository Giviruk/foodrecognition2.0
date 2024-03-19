import matplotlib.pyplot as plt
from keras.callbacks import ReduceLROnPlateau, ModelCheckpoint, EarlyStopping, CSVLogger
from keras.layers import Dense, Dropout, Conv2D, MaxPool2D, GlobalAveragePooling2D
from keras.models import Sequential
from keras.preprocessing.image import ImageDataGenerator
from keras.regularizers import l2

from food_classes import classes

food = classes
class_count = len(classes)

train_datagen = ImageDataGenerator(featurewise_center=False,
                                   samplewise_center=False,
                                   featurewise_std_normalization=False,
                                   samplewise_std_normalization=False,
                                   zca_whitening=False,
                                   rotation_range=5,
                                   width_shift_range=0.05,
                                   height_shift_range=0.05,
                                   shear_range=0.2,
                                   zoom_range=0.2,
                                   channel_shift_range=0.,
                                   fill_mode='nearest',
                                   cval=0.,
                                   horizontal_flip=True,
                                   vertical_flip=False,
                                   rescale=1 / 255)  # rescale to [0-1], add zoom range of 0.2x and horizontal flip
train_generator = train_datagen.flow_from_directory(
    "food-101/train",
    target_size=(224, 224),
    batch_size=64)
test_datagen = ImageDataGenerator(rescale=1 / 255)  # just rescale to [0-1] for testing set
test_generator = test_datagen.flow_from_directory(
    "food-101/test",
    target_size=(224, 224),
    batch_size=64)

model = Sequential()
model.add(
    Conv2D(filters=32, kernel_size=(5, 5), strides=2, padding='Same', activation='relu', input_shape=(224, 224, 3),
           kernel_initializer='he_normal'))
model.add(Conv2D(filters=32, kernel_size=(5, 5), strides=2, padding='Same', activation='relu',
                 kernel_initializer='he_normal'))
model.add(MaxPool2D(pool_size=(2, 2)))
model.add(Dropout(0.2))
model.add(Conv2D(filters=64, kernel_size=(3, 3), padding='Same', activation='relu', kernel_initializer='he_normal'))
model.add(Conv2D(filters=64, kernel_size=(3, 3), padding='Same', activation='relu', kernel_initializer='he_normal'))
model.add(MaxPool2D(pool_size=(2, 2)))
model.add(Dropout(0.2))
model.add(Conv2D(filters=128, kernel_size=(2, 2), padding='Same', activation='relu', kernel_initializer='he_normal'))
model.add(Conv2D(filters=128, kernel_size=(2, 2), padding='Same', activation='relu', kernel_initializer='he_normal'))
model.add(MaxPool2D(pool_size=(2, 2)))
model.add(Dropout(0.2))
model.add(Conv2D(filters=256, kernel_size=(2, 2), padding='Same', activation='relu', kernel_initializer='he_normal'))
model.add(Conv2D(filters=256, kernel_size=(2, 2), padding='Same', activation='relu', kernel_initializer='he_normal'))
model.add(GlobalAveragePooling2D())
model.add(Dense(512, activation="relu", kernel_initializer='he_normal'))
model.add(Dropout(0.2))
model.add(Dense(class_count, activation="softmax", kernel_initializer='he_normal', kernel_regularizer=l2()))

# callbacks
checkpointer = ModelCheckpoint(filepath='models/model.{epoch:02d}-{val_loss:.2f}.hdf5', verbose=1, save_best_only=True)
earlystopping = EarlyStopping(monitor='val_loss', min_delta=0.01, patience=20, mode='auto')
reduceLR = ReduceLROnPlateau(monitor='val_loss', factor=0.5, patience=10, mode='auto')
csv_logger = CSVLogger('logs/model.log')

model.compile(optimizer='Adam', loss="categorical_crossentropy", metrics=["accuracy"])
history = model.fit_generator(train_generator, steps_per_epoch=class_count*750 / 64,
                              validation_data=test_generator, validation_steps=class_count*250 / 64,
                              epochs=100, callbacks=[checkpointer, reduceLR, earlystopping, csv_logger])

acc = history.history['accuracy']
val_acc = history.history['val_accuracy']

loss = history.history['loss']
val_loss = history.history['val_loss']

# logs = pd.read_csv("logs/model.log", sep=',')
# acc = logs["accuracy"].tolist()
# val_acc = logs["val_accuracy"].tolist()
# loss = logs["loss"].tolist()
# val_loss = logs["val_loss"].tolist()

epochs_range = range(len(acc))

plt.figure(figsize=(8, 8))
plt.subplot(1, 2, 1)
plt.plot(epochs_range, acc, label='Training Accuracy')
plt.plot(epochs_range, val_acc, label='Validation Accuracy')
plt.legend(loc='lower right')
plt.title('Training and Validation Accuracy')

plt.subplot(1, 2, 2)
plt.plot(epochs_range, loss, label='Training Loss')
plt.plot(epochs_range, val_loss, label='Validation Loss')
plt.legend(loc='upper right')
plt.title('Training and Validation Loss')
plt.show()


print("final")