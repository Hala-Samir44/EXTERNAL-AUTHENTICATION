import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AddPermissionComponent } from '../add-permission/add-permission.component';

@Component({
  selector: 'app-permission-list',
  templateUrl: './permission-list.component.html',
  styleUrls: ['./permission-list.component.scss']
})
export class PermissionListComponent implements OnInit {
  Permissions: any[] = [];
  @ViewChild(AddPermissionComponent) child:any;
  isEdit=false;
  EditPermissionID=0;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getAllPermissions();
  }
  getAllPermissions() {
    this.http.get('https://localhost:44307/api/Permission/GetAllPermissions').subscribe(
      (res: any) => {
        console.log("Ss", res);
        this.Permissions = res.data;
        console.log("this.Permissions", this.Permissions);
      }
    )
  }
  permissionAdded(event: any) {

    if(this.isEdit){
      var index = this.Permissions.indexOf(this.Permissions.find(e=>e.id == this.EditPermissionID));
      this.Permissions[index]=event;
    }else{
      this.Permissions.push(event)
    }
  }
  EditPermissionModal(id:any){
    console.log("df",id);
    this.EditPermissionID= id;
    var permission = this.Permissions.find(e=>e.id == id);
    console.log("d",permission);
    this.child.doSomething(permission);
    this.isEdit=true;
  }

  DeletePermission(id:any){
   var data={
      id:id
    }
    this.http.post('https://localhost:44307/api/Permission/DeletePermission', data).subscribe(
      (res: any) => {
        if (res.status == 1) {
          var index = this.Permissions.indexOf(this.Permissions.find(e=>e.id == id));
      this.Permissions.splice(index,1);
        }
        console.log("this.res", res);
      }
    )

  }


}
