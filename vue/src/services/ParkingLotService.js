import axios from 'axios';

const http = axios.create({
    baseURL: "https://localhost:44315"
});


export default {
    getCustomerByTicket(ticketNumber) {
        return http.get(`/api/Customers/${ticketNumber}`);
    },
    getParkingSpotDetails(parkingSpotNumber) {
        return http.get(`/api/ParkingLot/${parkingSpotNumber}/Details`)
    },
    getAllCars() {
        return http.get('/api/Cars');
    },
    getTransactionAmount(ticketNumber) {
        return http.get(`/api/Transaction/${ticketNumber}/Amount`)
    },
    getTimeParked(ticketNumber) {
        return http.get(`/api/Transaction/${ticketNumber}/time`)
    },
    toggleNeedsPickedUp(ticketNumber) {
        return http.put(`/api/ParkingLot/ticket/${ticketNumber}`)
    },
    getCarsParkedInLot() {
        return http.get('/api/ParkingLot')
    },
    async postNewCar(newCar, newCustomer, valetId) {
        console.log(newCar);
        console.log(newCustomer);
        console.log(valetId);
        const [data1, newId] = await Promise.all([
            axios.post(`api/Cars`, newCar),
            axios.post(`api/Customers`, newCustomer)
        ]);
        data1;
        return await (await this.postNewOwners_Cars(newId.data, newCar.licensePlate)).data;

        /*
        const [ticketNumber] = await Promise.all([
           await this.postNewTransaction( valetId, associationId.data)
        ]);
        ticketNumber; */

    },
    postNewOwners_Cars(newCustomerId, newCarLicensePlate) {
        return http.post('api/Owners_Cars', { id: newCustomerId, licensePlate: newCarLicensePlate });
    },
    postNewTransaction(dto) {
        return http.post('api/Transaction', dto);
    },
    updateParkingLotWithNewCar(dto) {
        return http.put(`api/ParkingLot/spot/${dto.parkingSpotId}`, dto);
    },
    /*deleteCarsFromLot(licensePlate, valetID) {
        http.delete('/api/ParkingLot', licensePlate, valetID)
    }*/
    deleteCarsFromLot(licensePlate, valetID) {
        return http.delete('/api/ParkingLot', {
            params: {
                licensePlate: licensePlate,
                valetID: valetID
            },
            validateStatus: function(status) {
                // Define your custom validation logic for response status codes here
                // For example, return true for 2xx status codes, and false for others
                return status >= 200 && status < 300;
            }
        });
    },
    getListOfAvailableSpots() {
        return http.get('/AvailableSpots/list');
    },

    getAvailableSpots() {
        return http.get('/AvailableSpots');
    }
}