import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { inventory } from '../model/inventory';
import {Request} from '../model/request';
import { RequestProcessResult } from '../model/RequestProcessResult';
import { InventoryService } from '../services/inventory.service';
import { RequestService } from '../services/request.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  public loading: boolean = false;

  message: string = "";

  inventoryData: any;
  requestData: any;

  inventoryColumns: string[] = ["id", "name", "kernels"];
  requestColumns: string[] = ["id", "inventoryName","requested","process"]

  constructor(private inventoryService: InventoryService, 
    private requestService: RequestService) { }

  ngOnInit(): void {
    this.loadInventory();
    this.loadRequests();
  }

  loadInventory(){
    this.loading = true;

    this.inventoryService.getAll().subscribe((data:inventory[])=>{
      this.inventoryData = new MatTableDataSource(data);
    })

    this.loading = false;
  }

  loadRequests(){
    this.loading = true;

    this.requestService.getAll().subscribe((data:Request[])=>{
      this.requestData = new MatTableDataSource(data);
    })

    this.loading = false;
  }

  process(request: Request){
    this.requestService.process(request).subscribe((response:RequestProcessResult)=>{
      if(response.result)
        this.message = `${response.message} - Inventory: ${response.result.inventoryName} - Requested: ${response.result.requestedKernels} - New Balance: ${response.result.kernels}`;
      else
      this.message = `${response.message}`;

      this.loadInventory();
    });
  }

}
