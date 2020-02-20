%% Simple with one elstic element

runtime = 20;       % time of simulation [s]
h = 0.01;           % step size
kickAvgR = 0.2;% average number of kicks per second
kick_force = 1000;  % [N]
y0 = -0.3;          % initial position
v0 = 0.0;           % initial velocity

N = runtime / h;
u = (rand(1, N) <= kickAvgR / (N/runtime)) * kick_force;
z0 = [y0, v0];
[t, z] = ode2euler(@fun_baby, z0, u, N, h);

plot(t, z(1,:));

%% 2D

runtime = 20;       % time of simulation [s]
h = 0.01;           % step size
kickAvgR = 0.2;% average number of kicks per second
kick_force = 100;   % [N]
x0 = 0.0;           % initial x-position
y0 = -0.3;          % initial y-position
v0 = 0.0;           % initial velocity
theta = pi/2;       % initial angel

N = runtime / h;
uR = (rand(1, N) <= kickAvgR / (N/runtime)) * kick_force;
uL = (rand(1, N) <= kickAvgR / (N/runtime)) * kick_force;

z0 = [x0, v0*cos(theta), y0, v0*sin(theta)];
u = [uR; uL];
[t, z] = ode2euler2(@fun_x_acceleration, @fun_y_acceleration, z0, u, N, h);
 
kickR = find(uR);
kickL = find(uL);
plot(t, z(1,:), t, z(3,:));
hold on
plot(kickR*h, z(1,kickR), 'k^', kickL*h, z(1,kickL), 'ko');
plot(kickR*h, z(3,kickR), 'k^', kickL*h, z(3,kickL), 'ko');
title('Position of the kicking baby');
legend('x position', 'y posistion', 'right kick', 'left kick');

nKicks = [length(kickR), length(kickL)]

%% 2D with two elastic bands

clc
close all

runtime = 10;       % time of simulation [s]
h = 0.01;           % step size
kickAvgR = 0.2;     % right leg average number of kicks per second
kickAvgL = 0.2;     % left leg average number of kicks per second
kickForceR = 100;   % [N]
kickForceL = 100;   % [N]
x0 = 0.0;           % initial x-position
y0 = -0.3;          % initial y-position
v0 = 0.0;           % initial velocity
theta = pi/4;       % initial angle of motion

N = runtime / h;
uR = (rand(1, N) < kickAvgR / (N/runtime)) * kickForceR;
uL = (rand(1, N) < kickAvgL / (N/runtime)) * kickForceL;

z0 = [x0, v0*cos(theta), y0, v0*sin(theta)];
u = [uR; uL];
[t, z] = ode2euler3(@baby_acceleration, z0, u, N, h);

kickR = find(uR);
kickL = find(uL);

plot(t, z(1,:), t, z(3,:));
hold on
plot(kickR*h, z(1,kickR), 'k^', kickL*h, z(1,kickL), 'ko');
plot(kickR*h, z(3,kickR), 'k^', kickL*h, z(3,kickL), 'ko');
title('Position of the kicking baby');
legend('x position', 'y position', 'right kick', 'left kick');

nKicks = [length(kickR), length(kickL)]
%% Rotation

clc
close all

runtime = 10;       % time of simulation [s]
h = 0.01;           % step size
kickAvgR = 0.4;     % right leg average number of kicks per second
kickAvgL = 0.4;     % left leg average number of kicks per second
kickForceR = 100;   % [N]
kickForceL = 100;   % [N]
x0 = 0.0;           % initial x-position
y0 = -0.3;          % initial y-position
v0 = 0.0;           % initial velocity
theta = pi/4;       % initial angle of motion
angle = 0;          % initial angle offset
w0 = 0.0;           % initial rotation

N = runtime / h;
uR = (rand(1, N) < kickAvgR / (N/runtime)) * kickForceR;
uL = (rand(1, N) < kickAvgL / (N/runtime)) * kickForceL;

z0 = [x0, v0*cos(theta), y0, v0*sin(theta), angle, w0];
u = [uR; uL];
[t, z] = ode2euler4(@baby_rot_acceleration, z0, u, N, h);

kickR = find(uR);
kickL = find(uL);

plot(t, z(1,:), t, z(3,:), t, z(5,:));
hold on
plot(kickR*h, z(1,kickR), 'k^', kickL*h, z(1,kickL), 'ko');
plot(kickR*h, z(3,kickR), 'k^', kickL*h, z(3,kickL), 'ko');
title('Position of the kicking baby');
legend('x position', 'y position', 'angle', 'right kick', 'left kick', 'Location', 'best');

nKicks = [length(kickR), length(kickL)]

%createMovie('rotBandKick5', z, h)






