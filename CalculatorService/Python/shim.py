

import prediction
# sys.path.append(sys.path[0] + '\\price_prediction')
import sys



def RaiseArgumentException (message):
    raise ValueError("""
""" + message + """

    Output:                 [ 'price', 'price+tax', 'price+tax+freight', 'weight' ]
    Vendor:                 [ 'Glen', 'Sarah/Ella', 'Sunny', 'Eric' ]
    Material:               [ 
                                'PET/VMPET/PE',     'MOPP/VMPET/PE',    'KPET/VMPET/PE',
                                'PET/PE','KPET/PE', 'KPET/NY/VMPET/PE', 'PET/AL/PE', 
                                'PET/AL/NY/PE',     'MOPP/AL/PE',       'MOPP/PAPER/PE' 
                            ]
    Configuration:          [ '2-Seal', '3-Seal', '8-Seal', 'SUP' ]
    Print:                  [ 'Digital', 'Plate' ]
    Zipper:                 [ 'Yes', 'No', 'CR' ]
    Thickness (micrometers):<number>
    Width (mm):             <number>
    Length (mm):            <number>
    Bottom(Gusset) (mm):    <number>
    Quantity:               <number>
    """)
    
if len(sys.argv) != 12: RaiseArgumentException("The model considers 10 different features and 1 output argument, ergo 11 args are required:") # The shim.py is also an arg ignored for the purpose of this tally

Output = sys.argv[1]
Args = [sys.argv[2], sys.argv[3], sys.argv[4], sys.argv[5], sys.argv[6], int(sys.argv[7]), float(sys.argv[8]), float(sys.argv[9]), float(sys.argv[10]), int(sys.argv[11])]

if Output == "price":
    R = prediction.model_predict(Args)          # returns unit price.
 
elif Output == "price+tax":
    R = prediction.unit_price_with_tax(Args)    # This gives unit price including tax. The tax rate is given in 'input_file.csv' file in the model directory.
 
elif Output == "price+tax+freight":
    R = prediction.total_unit_price_with_freight(Args)  # This gives total unit price including tax and freight. The freight is calculated using com rate given in "input_file.csv" file in the model directory.
 
elif Output == "weight":
    R = prediction.total_weight(Args)           # This gives total weight. 
 
else: RaiseArgumentException("The Output argument has an invalid value: " + Output)


print("|||RESULT|||")
print(R)
print("|||RESULT|||")
