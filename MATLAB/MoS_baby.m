%% Simple with one elstic element

runtime = 20;       % time of simulation [s]
h = 0.01;           % step size
kicks_per_sec = 0.2;% average number of kicks per second
kick_force = 1000;  % [N]
y0 = -0.3;          % initial position
v0 = 0.0;           % initial velocity

N = runtime / h;
u = (rand(1, N) <= kicks_per_sec / (N/runtime)) * kick_force;
z0 = [y0, v0];
[t, z] = ode2euler(@fun_baby, z0, u, N, h);

plot(t, z(1,:));

%% Two dimensions

runtime = 20;       % time of simulation [s]
h = 0.01;           % step size
kicks_per_sec = 0.2;% average number of kicks per second
kick_force = 1000;  % [N]
x0 = 0.0;           % initial x-position
y0 = -0.3;          % initial y-position
v0 = 0.0;           % initial velocity
angle = pi/2;       % initial angel

N = runtime / h;
uR = (rand(1, N) <= kicks_per_sec / (N/runtime)) * kick_force;
uL = (rand(1, N) <= kicks_per_sec / (N/runtime)) * kick_force;

z0 = [x0, v0*cos(angle), y0, v0*sin(angle)];
u = [uR; uL];
[t, z] = ode2euler2(@fun_x_acceleration, @fun_y_acceleration, z0, u, N, h);
 
rKick = find(uR);
lKick = find(uL);
plot(t, z(1,:), t, z(3,:));
hold on
plot(rKick*h, z(1,rKick), 'k^', lKick*h, z(1,lKick), 'ko');
plot(rKick*h, z(3,rKick), 'k^', lKick*h, z(3,lKick), 'ko');
title('Position of the kicking baby');
legend('x position', 'y posistion', 'right kick', 'left kick');

nKicks = [length(rKick), length(lKick)]








