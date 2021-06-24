import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgbModal, ModalDismissReasons }
  from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-add-role',
  templateUrl: './add-role.component.html',
  styleUrls: ['./add-role.component.scss']
})
export class AddRoleComponent implements OnInit {
  closeResult = '';
  Permissions: any[] = [];
  title = "";
  description = "";
  PermissionIds: number[] = [];
  addOrEdit = "Add";
  roleId = 0;
  @Output() myEvent = new EventEmitter<any>();

  @ViewChild('edit') edi: ElementRef | undefined;


  constructor(private modalService: NgbModal, private http: HttpClient) { }

  ngOnInit(): void {
    this.getAllPermissions();
  }
  open(content: any) {
    this.modalService.open(content,
      { ariaLabelledBy: 'modal-basic-title', size: 'xl' }).result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        }, (reason) => {
          this.closeResult =
            `Dismissed ${this.getDismissReason(reason)}`;
        });
  }


  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  getAllPermissions() {
    this.http.get('https://localhost:44307/api/Permission/GetAllPermissions').subscribe(
      (res: any) => {
        this.Permissions = res.data;
        console.log("this.Permissions", this.Permissions);
      }
    )
  }

  PermissionsValue(perId: any) {
    if (this.PermissionIds.includes(perId)) {
      this.PermissionIds.splice(this.PermissionIds.indexOf(perId), 1)
    } else {
      this.PermissionIds.push(perId)
    }
    console.log("this.PermissionIds", this.PermissionIds);
  }
  submit() {
    var data = {
      id: this.roleId,
      title: this.title,
      description: this.description,
      permissions: this.PermissionIds
    }

    this.http.post('https://localhost:44307/api/Role/' + this.addOrEdit + 'Role', data).subscribe(
      (res: any) => {
        if (res.status == 1) {
          this.myEvent.emit(data);
          this.title = "";
          this.description = "";
          this.PermissionIds = [];
        }
        console.log("this.res", res);
      }
    )
  }

  doSomething(obj: any) {
    this.addOrEdit = "Edit"
    this.roleId = obj.id;
    this.title = obj.title;
    this.description = obj.description;
    this.PermissionIds = obj.permissions;
    this.edi?.nativeElement.click();
  }
}
