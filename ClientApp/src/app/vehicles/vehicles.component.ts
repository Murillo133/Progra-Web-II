import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css']
})
export class VehiclesComponent {

  public vehicles: Vehicle[];
  public vehicle: Vehicle;
  public showForm = false;
  public show_edit_form = false;
  public id_edit_form = "";

  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
    this.refresh();
  }

  refresh(){
    this.http.get<Vehicle[]>(this.baseUrl + 'api/vehicles').subscribe(result => {
      this.vehicles = result;
      this.vehicle = {
        color: "#FFFFFF",
        manufacturer: "",
        year: 2020,
        mileage: 0
      };
      this.showForm = false;
    }, error => console.error(error));
  }

  save() {
    this.http.post(this.baseUrl + 'api/vehicles', this.vehicle).subscribe(() => {
      this.refresh();
    }, error => console.error(error));
  }

  delete(id:string) {
    this.http.delete(this.baseUrl + 'api/vehicles/'+id).subscribe(() => {
      this.refresh();
    }, error => console.error(error));
  }


  editForm(vehiToEdit:Vehicle, id:string){

    this.showForm = true;
    this.show_edit_form = true;
    this.id_edit_form = id;
    this.vehicle.color = vehiToEdit.color;
    this.vehicle.manufacturer = vehiToEdit.manufacturer;
    this.vehicle.year = vehiToEdit.year;
    this.vehicle.mileage = vehiToEdit.mileage;

  }


  update(id:string){
  
  this.showForm = false;
  this.show_edit_form = false;
  
  this.http.patch(this.baseUrl + 'api/vehicles/' + id, [
      {
        "op": "replace", "path": "/color", "value": this.vehicle.color
      },
      {
        "op": "replace", "path": "/manufacturer", "value": this.vehicle.manufacturer
      },
      {
        "op": "replace", "path": "/year", "value": this.vehicle.year
      },
      {
        "op": "replace", "path": "/mileage", "value": this.vehicle.mileage
      }
    ]).subscribe(() => {
      this.refresh();
    }, error => console.error(error));

  }



}

interface Vehicle {

  color: string;
  year: number;
  manufacturer: string;
  mileage: number;
}
