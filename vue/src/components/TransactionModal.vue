<template>
    <div id="modal-container">
    <form  class="newVehicleForm" action="" v-on:submit.prevent="submitNewCarRecord()">
        <ul>
            <li class="title">Parking Spots: </li>
            <li>
                <div v-for="item in availableSpots" class="parkingSpots" :key="item" >
                    <input required v-model="parkingSpot" type="radio" id="radioButton" :value=item />
                    <label for="radioButton">{{item}}                     </label>
                </div>
            </li>
            <li>
                <input type="text" placeholder="Customer First Name" v-model="newCustomer.firstName" required>
            </li>
            <li>
                <input type="text" placeholder="Customer Last Name" v-model="newCustomer.lastName" required>
            </li>
            <li>
                <input type="text" placeholder="Customer Phone Number" v-model="newCustomer.phoneNumber" required>
            </li>
            <li>
                <input type="text" placeholder="License Plate" v-model="newCar.licensePlate" required>
            </li>
            <li>
                <input type="text" placeholder="Make" v-model="newCar.make" required>
            </li>
            <li>
                <input type="text" placeholder="Model" v-model="newCar.model" required>
            </li>
            <li>
                <input type="text" placeholder="VIN" v-model="newCar.vin" required>
            </li>
            <li>
                <input type="text" placeholder="Color" v-model="newCar.color" required>
            </li>
            
        </ul>
      <button class="pickup-button">Submit new car</button>
    </form>
    <br/>
    </div>
</template>

<script>
import ParkingLotService from '../services/ParkingLotService'

export default {
    async mounted() {
        await this.getListOfSpots();
    },
    methods: {
        async getListOfSpots(){
            const response = await ParkingLotService.getListOfAvailableSpots();
            this.availableSpots = response.data;
        },
        async submitNewCarRecord(){
            const associationId = await ParkingLotService.postNewCar(this.newCar, this.newCustomer);
            console.log(associationId);
            const ticketNumber = (await ParkingLotService.postNewTransaction({"valetId":this.$store.state.user.userId, "AssociationId":associationId})).data.ticketNumber;
            await ParkingLotService.updateParkingLotWithNewCar({"parkingSpotId": Number(this.parkingSpot), "ticketNumber":ticketNumber});
            this.$emit('updateParkingLot');
            this.newCustomer = {};
            this.newCar = {};
            this.parkingSpot = "";
            await this.getListOfSpots();
        }
    },
    name: "transaction-modal",
    data() {
        return {
            availableSpots: [],
            newTransaction: {
            },
            parkingSpot: "",
            newCar: {
                "licensePlate": "",
                "make": "",
                "model": "",
                "vin": "",
                "color": "",
                "needsPickedUp": false
            },
            newCustomer: {
                "firstName":"",
                "lastName":"",
                "phoneNumber": ""
            }
        }
    }
}
</script>

<style>
#modal-container {
    text-align: center;
    width: 100%;
    color: white;
    background: rgba(255, 255, 255, 0.1); 
  border-radius: 10px; 
  backdrop-filter: blur(3px); 
  border: 1px solid rgba(255, 255, 255, 0.2);
  max-width: 350px;
  width: 90%;
  padding: 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

  margin: 25px;
}

.title {
    font-size: 25px;
    color: white;
}

li {
    padding: 5px;
}

li > input {
    height: 2vw;
}


.pickup-button {
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #ffc700;
  color: black;
  font-weight: normal;
  font-size: 15px;
  cursor: pointer;
  float: center;
}
.parkingSpots{
    display: inline;
}
.newVehicleForm{
    width: 100%;
}

@media screen and (max-width: 1300px) {
  li > input {
    height: 4vw;
}
}

@media screen and (max-width: 768px) {
  li > input {
    height: 5vw;
}
}


@media screen and (max-width: 450px) {
  li > input {
    height: 9vw;
}
}

</style>