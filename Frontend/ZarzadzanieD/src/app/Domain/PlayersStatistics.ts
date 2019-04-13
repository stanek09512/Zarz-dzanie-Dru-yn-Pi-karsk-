export interface UpdateStatisticPlayersInMatch {
  playersStatisticInMatch: Array<playersStatisticInMatch>;
}


// tslint:disable-next-line:class-name
export interface playersStatisticInMatch {
  id?: number;
  name?: string;
  surname?: string;
  position?: string;
  matches?: number;
  goals?: number;
  assists?: number;
  yellowCard?: number;
  redCard?: number;
  playInMatch?: boolean;
  isEditable?: boolean;
}
