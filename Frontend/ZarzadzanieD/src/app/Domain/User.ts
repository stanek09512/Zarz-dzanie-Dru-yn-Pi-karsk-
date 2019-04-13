export interface User {
  id?: number;
  username?: string;
  role?: string;
}


export interface UserNameDto {
  id?: number;
  userName?: string;
}

export interface UserPasswordDto {
  id?: number;
  Password1?: string;
  Password2?: string;
}

export interface UserRoleDto {
  id?: number;
  Role?: string;
}
