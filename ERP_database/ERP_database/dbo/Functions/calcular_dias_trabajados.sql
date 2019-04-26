 CREATE function [dbo].[calcular_dias_trabajados]
 (
 @Fecha_inicio date,
  @Fecha_fin date,
  @fecha_ingreso date,
  @em_status varchar(10),
  @Fecha_salida date
 )
 returns int 
 as
 begin 
   declare @dias int

   if(@em_status='EST_ACT')
   set @dias= iif(@fecha_ingreso<=@Fecha_inicio,DATEDIFF(day ,@Fecha_inicio, @Fecha_fin)+1, DATEDIFF(day ,@fecha_ingreso, @Fecha_fin))
   if(@em_status='EST_PLQ')
   set @dias= DATEDIFF(day ,@Fecha_inicio, @Fecha_salida)+1

   if(@dias>30)
   set @dias=30
   if((@dias=28 or @dias=29 and datepart(MONTH, @Fecha_inicio)=2) )
   set @dias=30
   if(@dias < 30 and MONTH(@Fecha_inicio) != 2 and @em_status<>'EST_PLQ')
   set @dias = @dias+1

   return @dias
 end;