function [ax, ay, alpha] = babyAccelerations(x, vx, y, vy, angle, w, u)

m = 9.0;                    % baby weight [kg]
l = 0.75;                   % baby length [m]
r = 0.15;                   % baby radius [m]
kickAngleR = 3*pi/8 + angle;% kick angle right leg
kickAngleL = 5*pi/8 + angle;% kick angle left leg

k = 145.5;                  % spring constant [N/m]
b = 8.0;                    % dampening [Ns/m]
bw = 4.0;                   % dampening for rotation [Nms^2/rad]
bandStartR = [-0.3, 2.0];   % where the elastic band is fixed
bandEndR = [-r, 0.0];       % where band stops without weight
bandStartL = [0.3, 2.0];    % where the elastic band is fixed
bandEndL = [r, 0.0];        % where band stops without weight
%*************************************************************************

g = 9.82;                   % gravitation [m/s^2] 
J = 1/4*m*r^2 + 1/12*m*l^2; % moment of inertia, solid cylinder

% ÄNDING: legPos är förskjutet för att passa vart kraften går från benet
% till resten av kroppen. Innan så gjorde den stora vinkeln att rotationen
% gick åt fel håll.
R = [cos(angle), -sin(angle); sin(angle), cos(angle)];
legPosR = (R*[-r/2;0.0])' + [x,y]; % position of right leg after rot
legPosL = (R*[r/2;0.0])' + [x,y];  % position of left leg after rot
newEndR = (R*[bandEndR(1); bandEndR(2)])' + [x,y];% band position after rot
newEndL = (R*[bandEndL(1); bandEndL(2)])' + [x,y];% band position after rot

bandLengthR = pdist([bandStartR; bandEndR], 'euclidean');
bandLengthL = pdist([bandStartL; bandEndL], 'euclidean');
bandStretchR = pdist([bandStartR; newEndR], 'euclidean');
bandStretchL = pdist([bandStartL; newEndL], 'euclidean');

stretchDistR = bandLengthR - bandStretchR;
stretchDistL = bandLengthL - bandStretchL;
if(stretchDistR > 0)
    stretchDistR = 0;
end
if(stretchDistL > 0)
    stretchDistL = 0;
end

bandAngleR = atan2((bandStartR(2)-newEndR(2)), ... 
    (bandStartR(1)-newEndR(1)));
bandAngleL = atan2((bandStartL(2)-newEndL(2)), ...
    (bandStartL(1)-newEndL(1)));

bandForceR = k*stretchDistR;
bandForceL = k*stretchDistL;

% Force depending on height
ux = (u(1)*cos(kickAngleR) + u(2)*cos(kickAngleL))*(1-(exp(min(y,0))));

uy = (u(1)*sin(kickAngleR) + u(2)*sin(kickAngleL))*(1-(exp(min(y,0))));

% Translation
ax = (1/m)*(ux - cos(bandAngleR)*bandForceR ...
    - cos(bandAngleL)*bandForceL - b*vx);

ay = (1/m)*(uy - sin(bandAngleR)*bandForceR ...
    - sin(bandAngleL)*bandForceL - b*vy - m*g);

% Rotation
torqueKickR = getTorque(u(1), legPosR, kickAngleR, [x,y]);
torqueKickL = getTorque(u(2), legPosL, kickAngleL, [x,y]);
torqueBandR = getTorque(bandForceR, newEndR, bandAngleR, [x,y]);
torqueBandL = getTorque(bandForceL, newEndL, bandAngleL, [x,y]);

%ÄNDRING: Eftersom kraften för banden är negativ behöver torque vara negativ 
alpha = (1/J)*(torqueKickR + torqueKickL - torqueBandR - torqueBandL - bw*w);

end