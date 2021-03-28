The model considers 10 different features:
Vendor: ['Glen', 'Sarah', 'Other']
Material: ['PET/VMPET/PE', 'MOPP/VMPET/PE', 'KPET/VMPET/PE', 'PET/PE','KPET/PE', 'KPET/NY/VMPET/PE', 'PET/AL/PE', 'PET/AL/NY/PE', 'MOPP/AL/PE', 'MOPP/PAPER/PE']
Configuration: ['2-Seal', '3-Seal', '8-Seal', 'SUP']
Print: ['Digital', 'Plate']
Zipper: ['Yes', 'No', 'CR']
Thickness (micrometers): 
Width (mm): 
Length (mm):
Bottom(Gusset) (mm):
Quantity: 

In order to predict the unit price from model first make a list of input features in following format:

 [Vendor, Material, Configuration, Print, Zipper, Thickness, Width, Length, Bottom, Quantity]

E.g. X = ['Glen','PET/PE','3-Seal','Plate', 'Yes', 110, 76, 127,0,100000]

Then use the model_predict method from prediction.py to make the prediction:
Run the following command in Python


import prediction

X = ['Glen','PET/PE','3-Seal','Plate', 'Yes', 110, 76, 127,0,100000] # Feel free to try any combination of features.

prediction.model_predict(X)       # this should output unit price. 

prediction.unit_price_with_tax(X)   # This gives unit price including tax. The tax rate is given in 'input_file.csv' file in the model directory.

prediction.total_unit_price_with_freight(X)  # This gives total unit price including tax and freight. The freight is calculated using com rate given in "input_file.csv" file in the model directory.

prediction.total_weight(X). # This gives total weight. 


Python package: 

In order to install the python package "prediction", unpack the zip file "price_prediction_v3" and "cd" to the directory containing the setup.py file. Then run the following command in the terminal:

python setup.py install

